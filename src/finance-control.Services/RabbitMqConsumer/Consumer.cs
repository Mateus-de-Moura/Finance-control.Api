using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;


namespace finance_control.Services.RabbitMqConsumer
{
    public class Consumer
    {
        private readonly string _hostName = "localhost";
        private readonly string _queueName = "expenses";

        public async Task StartListeningAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            Console.WriteLine($"Ouvindo a fila: {_queueName}");

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Mensagem recebida: {message}");

                ProcessMessage(message);
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queue: _queueName,
                                  autoAck: true,
                                  consumer: consumer);

            Console.WriteLine("Pressione [enter] para sair.");
            await Task.Delay(-1, cancellationToken);
        }


        private void ProcessMessage(string message)
        {
            Console.WriteLine($"Processando a mensagem: {message}");
            Thread.Sleep(1000);
            Console.WriteLine("Mensagem processada com sucesso!");
        }
    }
}
