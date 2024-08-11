using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

// Hello World Tutorial

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
const string QUEUE_NAME = "ttl_queue";

// Set the message TTL to 5000 milliseconds (5 seconds)
var aargs = new Dictionary<string, object>
{
    { "x-message-ttl", 5000 } // Message TTL in milliseconds
};

// Declare the queue with the TTL argument
channel.QueueDeclare(queue: QUEUE_NAME,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: aargs);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");

    // Manually acknowledge the message
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};
// 消费消息，会持续阻塞
channel.BasicConsume(queue: QUEUE_NAME,
                     autoAck: false,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();