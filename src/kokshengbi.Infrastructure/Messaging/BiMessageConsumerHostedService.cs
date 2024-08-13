using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Logging;

namespace kokshengbi.Infrastructure.Messaging
{
    public class BiMessageConsumerHostedService : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IBiMessageConsumer _biMessageConsumer;
        private readonly ILogger<BiMessageConsumerHostedService> _logger;

        public BiMessageConsumerHostedService(
            IBiMessageConsumer biMessageConsumer,
            ILogger<BiMessageConsumerHostedService> logger,
            IConnection connection)
        {
            _biMessageConsumer = biMessageConsumer;
            _logger = logger;
            _channel = connection.CreateModel();

            _channel.ExchangeDeclare(BiMqConstant.BI_EXCHANGE_NAME, ExchangeType.Direct);
            _channel.QueueDeclare(BiMqConstant.BI_QUEUE_NAME, true, false, false, null);
            _channel.QueueBind(BiMqConstant.BI_QUEUE_NAME, BiMqConstant.BI_EXCHANGE_NAME, BiMqConstant.BI_ROUTING_KEY);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var deliveryTag = ea.DeliveryTag;

                try
                {
                    await _biMessageConsumer.ConsumeMessage(message, deliveryTag);
                    _channel.BasicAck(deliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming message.");
                    _channel.BasicNack(deliveryTag, false, false);
                }
            };

            _channel.BasicConsume(queue: BiMqConstant.BI_QUEUE_NAME,
                                  autoAck: false,
                                  consumer: consumer);

            return Task.CompletedTask;
        }
        public override void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
