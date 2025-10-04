using System.Text.Json;
using BackgroundService.Entity;
using BackgroundService.Enum;
using BackgroundService.Infra;
using BackgroundService.RabbitMqPublisher;
using BackgroundService.Services.Mail;
using Microsoft.EntityFrameworkCore;

namespace BackgroundService.Services
{
    public class ExpensesService(FinanceControlContex context, MailSender mailSender)
    {
        private readonly FinanceControlContex _context = context;
        private readonly MailSender _mailSender = mailSender;
        public async Task CheckExpensesAndNotify()
        {
            var UsersSend = new List<Guid>();

            var expenses = await _context.Expenses
                .Include(x => x.User)
                .Where(x => x.DueDate.Date <= DateTime.Now.Date && x.Status != InvoicesStatus.Pago).ToListAsync();

            foreach (var item in expenses)
            {
                var notification = await _context.Notify
                    .AsNoTracking()
                    .Where(x => x.ExpensesId.Equals(item.Id) && x.UserId.Equals(item.User.Id))
                    .FirstOrDefaultAsync();

                if (notification is null)
                {
                    string message = string.Empty;
                    int days = Math.Abs((item.DueDate - DateTime.Now.Date).Days);
                    string priority = "";

                    if (item.DueDate.Date < DateTime.Now.Date)
                    {
                        message = $"Despesa em Atraso a {days} dias.";
                        priority = "Alta";
                    }
                    else
                    {
                        message = "Você possui uma despesa que vence hoje. Atente-se para não atrasar";
                        priority = "Média";
                    }

                    var notificacao = new Notify
                    {
                        ExpensesId = item.Id,
                        Message = message,
                        Priority = priority,
                        UserId = item.User.Id
                    };

                    await _context.Notify.AddAsync(notificacao);

                    var rowsAffected = await _context.SaveChangesAsync();

                    if (rowsAffected > 0)
                    {
                        if (!UsersSend.Contains(item.User.Id))
                        {
                            var payload = new NotificationMessage
                            {
                                UserId = item.User.Id.ToString(),
                                Text = $"{item.User.UserName}, voce possui uma nova notificação"
                            };

                            var json = JsonSerializer.Serialize(payload);
                            UsersSend.Add(item.User.Id);

                            await Publisher.SendMessage(json);

                            _ = Task.Run(async () =>
                            {
                                await _mailSender.Send();
                            });
                        }
                    }
                }
            }
        }

        public async Task RegisterExpensesIsRecurrent()
        {
            var date = DateTime.Now;

            var lastJobExecution = await _context.JobExecutionLog
                .Where(x => x.Last_Execution.Month.Equals(date.Month))
                .FirstOrDefaultAsync();

            if (lastJobExecution is not null)
                return;

            await _context.JobExecutionLog.AddAsync(new JobExecutionLog
            {
                Id = Guid.NewGuid(),
                Last_Execution = DateTime.Now,
            });

            // o job sempre roda todo  dia 30,  pra isso é preciso  obter as despesas recorrentes apenas do  mes em  que  o job esta rodando.
            var expensesRecurrent = await _context.Expenses
                .Where(x => x.IsRecurrent && x.DueDate.Month.Equals(date.Month))
                .ToListAsync();

            if (expensesRecurrent is not null && expensesRecurrent.Any())
            {
                var newExpensesToMonth = expensesRecurrent.Select(expense =>
                {
                    return new Expenses
                    {
                        Id = Guid.NewGuid(),
                        Active = expense.Active,
                        Value = expense.Value,
                        IsDeleted = expense.IsDeleted,
                        IsRecurrent = expense.IsRecurrent,
                        Status = expense.Status,
                        CategoryId = expense.CategoryId,
                        UserId = expense.UserId,
                        Description = expense.Description,
                        DueDate = expense.DueDate.AddMonths(1),
                    };
                }).ToList();

                await _context.AddRangeAsync(newExpensesToMonth);

                var rowsAffected = await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteOldLocationData()
        {
            var allData = await _context.LoginLocationData.ToListAsync();

            var groupedByEmail = allData
                 .GroupBy(x => x.EmailRequest);

            var recordsToDelete = new List<LoginLocationData>();

            foreach (var group in groupedByEmail)
            {
                var oldRecords = group
                    .OrderByDescending(x => x.AccessDate)
                    .Skip(10)
                    .ToList();

                recordsToDelete.AddRange(oldRecords);
            }

            if (recordsToDelete is not null && recordsToDelete.Any())
            {
                _context.LoginLocationData.RemoveRange(recordsToDelete);
                await _context.SaveChangesAsync();
            }
        }

    }
}
