using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kokshengbi.Infrastructure.Messaging
{
    public class BiMessageConsumerHostedService : BackgroundService
    {
        private readonly BiMessageConsumer _consumer;

        public BiMessageConsumerHostedService(BiMessageConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Adjust hostname as needed
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(BiMqConstant.BI_QUEUE_NAME, true, false, false, null);
            channel.QueueBind(BiMqConstant.BI_QUEUE_NAME, BiMqConstant.BI_EXCHANGE_NAME, BiMqConstant.BI_ROUTING_KEY);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                ulong deliveryTag = ea.DeliveryTag;

                try
                {
                    await _consumer.ConsumeMessage(message, deliveryTag);
                    // Acknowledge message
                    channel.BasicAck(deliveryTag, false);
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    // Optionally Nack the message
                    channel.BasicNack(deliveryTag, false, false);
                }
            };

            channel.BasicConsume(queue: BiMqConstant.BI_QUEUE_NAME, autoAck: false, consumer: consumer);

            // Keep the service running
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken); // Adjust as needed
            }
        }
    }
}
