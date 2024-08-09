using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
const string EXCHANGE_NAME = "direct-exchange";

channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Direct);

// Declare the first queue and bind it with the routing key "xiaoyu"
var queueName1 = "xiaoyu_queue";
channel.QueueDeclare(queue: queueName1,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
channel.QueueBind(queue: queueName1,
                  exchange: EXCHANGE_NAME,
                  routingKey: "xiaoyu");

// Declare the second queue and bind it with the routing key "xiaopi"
var queueName2 = "xiaopi_queue";
channel.QueueDeclare(queue: queueName2,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
channel.QueueBind(queue: queueName2,
                  exchange: EXCHANGE_NAME,
                  routingKey: "xiaopi");


Console.WriteLine(" [*] Waiting for messages.");

// Set up the consumer for the "xiaoyu_queue"
var consumer1 = new EventingBasicConsumer(channel);
consumer1.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($" [xiaoyu] Received '{routingKey}':'{message}'");
};
channel.BasicConsume(queue: queueName1,
                     autoAck: true,
                     consumer: consumer1);

// Set up the consumer for the "xiaopi_queue"
var consumer2 = new EventingBasicConsumer(channel);
consumer2.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($" [xiaopi] Received '{routingKey}':'{message}'");
};
channel.BasicConsume(queue: queueName2,
                     autoAck: true,
                     consumer: consumer2);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();