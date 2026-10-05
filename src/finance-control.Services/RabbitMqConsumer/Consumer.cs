using finance_control.Services.SignalR;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace finance_control.Services.RabbitMqConsumer
{
    public class NotificationPayload
    {
        public string UserId { get; set; } = default!;
        public string Text { get; set; } = default!;
    }

    public class Consumer(IHubContext<NotificationHub> hub, ILogger<Consumer> logger) : IConsumer<NotificationPayload>
    {
        private readonly IHubContext<NotificationHub> _hub = hub;
        private readonly ILogger<Consumer> _logger = logger;

        public async Task Consume(ConsumeContext<NotificationPayload> context)
        {
            var payload = context.Message;

            if (string.IsNullOrWhiteSpace(payload.UserId))
            {
                _logger.LogWarning("Mensagem ignorada: UserId nao informado.");
                return;
            }

            var connectionId = NotificationHub.GetConnectionId(payload.UserId);

            if (connectionId is not null)
            {
                await _hub.Clients.Client(connectionId)
                    .SendAsync("ReceiveNotification", payload.Text, context.CancellationToken);

                return;
            }

            _logger.LogInformation("Usuario {UserId} nao esta conectado no SignalR.", payload.UserId);
        }
    }
}
