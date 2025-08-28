using System.Text.Json;
using BackgroundService.Entity;
using BackgroundService.Enum;
using BackgroundService.Infra;
using BackgroundService.RabbitMqPublisher;
using Microsoft.EntityFrameworkCore;

namespace BackgroundService.Services
{
    public class ExpensesService(FinanceControlContex context)
    {       
        private readonly FinanceControlContex _context = context;
        public  async Task CheckExpensesAndNotify()
        {
            var expenses = await _context.Expenses
                .Include(x => x.User)
                .Where(x => x.DueDate.Date < DateTime.Now.Date && x.Status == InvoicesStatus.Vencido).ToListAsync();

            foreach (var item in expenses)
            {               
                var payload = new NotificationMessage
                {
                    UserId = item.User.Id.ToString(), 
                    Text = $"{item.User.UserName}, voce possui uma nova notificação"
                };

                var json = JsonSerializer.Serialize(payload);

                await Publisher.SendMessage(json);
                break;
            }                   
        }

        public async Task registerExpensesIsRecurrent()
        {
            var date = DateTime.Now;

            var lastJobExecution = await _context.JobExecutionLog.Where(x => x.Last_Execution.Month.Equals(date.Month)).FirstOrDefaultAsync(); 

            if(lastJobExecution is not null)
                return;

            await _context.JobExecutionLog.AddAsync(new JobExecutionLog
            {
                Id = Guid.NewGuid(),
                Last_Execution = DateTime.Now,
            });

            // o job sempre roda todo  dia 30,  pra isso é preciso  obter as despesas recorrentes apenas do  mes em  que  o job esta rodando.
            var expensesRecurrent = await _context.Expenses
                .Where(x => x.IsRecurrent &&  x.DueDate.Month.Equals(date.Month))
                .ToListAsync();

            if(expensesRecurrent is not null && expensesRecurrent.Any())
            {
                var newExpensesToMonth = expensesRecurrent.Select(expense =>
                {
                    return  new Expenses
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
                        ProofPath = expense.ProofPath,
                        DueDate = expense.DueDate.AddMonths(1),
                    };
                }).ToList();

                await _context.AddRangeAsync(newExpensesToMonth);

                var rowsAffected = await _context.SaveChangesAsync();
            }
        }
      
    }  
}
