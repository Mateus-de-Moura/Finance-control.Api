using System.Text.Json;
using BackgroundService.Entity;
using BackgroundService.Infra;
using BackgroundService.RabbitMqPublisher;
using Microsoft.EntityFrameworkCore;

namespace BackgroundService.Services
{
    public class ExpensesService(FinanceControlContex context)
    {       
        private readonly FinanceControlContex _contex = context;
        public  async Task CheckExpensesAndNotify()
        {

            var expenses = await _contex.Expenses
                .Include(x => x.User)
                .Where(x => x.DueDate.Date < DateTime.Now.Date).ToListAsync();

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
        public class NotificationMessage
        {
            public string UserId { get; set; } = default!;
            public string Text { get; set; } = default!;
        }
    }  
}
