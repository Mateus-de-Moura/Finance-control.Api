using RabbitMQ.Client;
using System.Text;

namespace BackgroundService.RabbitMqPublisher
{
    public static class Publisher
    {     
        private static readonly string _hostName =
            Environment.GetEnvironmentVariable("RabbitMq__Host") ?? "localhost";
        private static readonly string _queueName = "expenses";

        public static async Task SendMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
           
           await channel.QueueDeclareAsync(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message); 
            
            await channel.BasicPublishAsync(exchange: string.Empty,
                                 routingKey: _queueName,                             
                                 body: body);

            Console.WriteLine($"Mensagem enviada: {message}");
        }
    }
}
