using BackgroundService.RabbitMqPublisher;

namespace BackgroundService.Services
{
    public static class ExpensesService
    {
        public static async Task CheckExpensesAndNotify()
        {
           await Publisher.SendMessage("Executando job!");           
        }
    }
}
