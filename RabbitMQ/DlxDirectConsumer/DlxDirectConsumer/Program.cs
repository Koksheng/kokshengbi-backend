using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
const string DEAD_EXCHANGE_NAME = "dlx-direct-exchange";
const string WORK_EXCHANGE_NAME = "direct2-exchange";

// Declare the dead letter exchange
channel.ExchangeDeclare(exchange: DEAD_EXCHANGE_NAME, type: ExchangeType.Direct);

// Declare the working exchange
channel.ExchangeDeclare(exchange: WORK_EXCHANGE_NAME, type: ExchangeType.Direct);

// Declare the first queue with DLX parameters
var queueName1 = "xiaodog_queue";
var args1 = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", DEAD_EXCHANGE_NAME },
            { "x-dead-letter-routing-key", "waibao" }
        };
channel.QueueDeclare(queue: queueName1, durable: true, exclusive: false, autoDelete: false, arguments: args1);
channel.QueueBind(queue: queueName1, exchange: WORK_EXCHANGE_NAME, routingKey: "xiaodog");

// Declare the second queue with DLX parameters
var queueName2 = "xiaocat_queue";
var args2 = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", DEAD_EXCHANGE_NAME },
            { "x-dead-letter-routing-key", "laoban" }
        };
channel.QueueDeclare(queue: queueName2, durable: true, exclusive: false, autoDelete: false, arguments: args2);
channel.QueueBind(queue: queueName2, exchange: WORK_EXCHANGE_NAME, routingKey: "xiaocat");

Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

// Set up the consumer for the "xiaodog_queue"
var consumer1 = new EventingBasicConsumer(channel);
consumer1.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($" [xiaodog] Received '{routingKey}':'{message}'");

    // Reject the message and do not requeue it
    channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
};
channel.BasicConsume(queue: queueName1, autoAck: false, consumer: consumer1);

// Set up the consumer for the "xiaocat_queue"
var consumer2 = new EventingBasicConsumer(channel);
consumer2.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($" [xiaocat] Received '{routingKey}':'{message}'");

    // Reject the message and do not requeue it
    channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
};
channel.BasicConsume(queue: queueName2, autoAck: false, consumer: consumer2);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();