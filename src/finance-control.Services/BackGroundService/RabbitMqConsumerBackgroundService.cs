using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Services.RabbitMqConsumer;
using Microsoft.Extensions.Hosting;

namespace finance_control.Services.BackGroundService
{
    public class RabbitMqConsumerBackgroundService(Consumer consumer) : BackgroundService
    {
        private readonly Consumer _consumer = consumer;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {           
            await Task.Run(() => _consumer.StartListeningAsync(stoppingToken));
        }
    }
}
