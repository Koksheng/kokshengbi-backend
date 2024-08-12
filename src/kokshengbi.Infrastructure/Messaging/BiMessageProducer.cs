using kokshengbi.Application.Common.Interfaces.Messaging;
using RabbitMQ.Client;
using System.Text;

namespace kokshengbi.Infrastructure.Messaging
{
    public class BiMessageProducer : IBiMessageProducer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public BiMessageProducer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Adjust hostname as needed
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(BiMqConstant.BI_EXCHANGE_NAME, ExchangeType.Direct);
            _channel.QueueDeclare(BiMqConstant.BI_QUEUE_NAME, true, false, false, null);
            _channel.QueueBind(BiMqConstant.BI_QUEUE_NAME, BiMqConstant.BI_EXCHANGE_NAME, BiMqConstant.BI_ROUTING_KEY);
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: BiMqConstant.BI_EXCHANGE_NAME,
                                  routingKey: BiMqConstant.BI_ROUTING_KEY,
                                  basicProperties: null,
                                  body: body);
        }
    }
}
