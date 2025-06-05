using System;
using System.Text;
using System.Text.Json;
using finance_control.Services.SignalR;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace finance_control.Services.RabbitMqConsumer
{
    public class Consumer(IHubContext<NotificationHub> hub)
    {
        private readonly string _hostName = "localhost";
        private readonly string _queueName = "expenses";
        private readonly IHubContext<NotificationHub> _hub = hub;

        public async Task StartListeningAsync(CancellationToken cancellationToken)
        {
            try
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

                    Task.Run(() => ProcessMessage(message));                 

                    return Task.CompletedTask;
                };

                await channel.BasicConsumeAsync(queue: _queueName,
                                      autoAck: true,
                                      consumer: consumer);

                Console.WriteLine("Pressione [enter] para sair.");
                await Task.Delay(-1, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Mensageria parada. Não foi possível iniciar a escuta da fila.");
                Console.WriteLine($"Detalhes: {ex.Message}");
            }
        }


        private async Task ProcessMessage(string message)
        {
            var data = JsonSerializer.Deserialize<NotificationPayload>(message);

            var connectionId = NotificationHub.GetConnectionId(data.UserId);
            if (connectionId != null)
            {
                await _hub.Clients.Client(connectionId)
                          .SendAsync("ReceiveNotification", data.Text);
            }
            else
            {
                Console.WriteLine($"⚠️ Usuário {data.UserId} não está conectado.");
            }
        }

        private class NotificationPayload
        {
            public string UserId { get; set; } = default!;
            public string Text { get; set; } = default!;
        }
    }
}
