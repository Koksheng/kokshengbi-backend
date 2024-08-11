// Time To Live Tutorial

using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
const string QUEUE_NAME = "ttl_queue";

//var aargs = new Dictionary<string, object>
//{
//    { "x-message-ttl", 5000 } // Message TTL in milliseconds
//};

// Declare the queue (to ensure it exists)
//channel.QueueDeclare(queue: QUEUE_NAME,
//                     durable: false,
//                     exclusive: false,
//                     autoDelete: false,
//                     arguments: null);

var message = "Hello World!";
var body = Encoding.UTF8.GetBytes(message);

// Set message expiration to 1000 milliseconds (1 second)
//var properties = channel.CreateBasicProperties();
//properties.Expiration = "1000"; // Message TTL in milliseconds

// Publish the message to a specified exchange and routing key
//channel.BasicPublish(exchange: "my-exchange",
//                     routingKey: "routing-key",
//                     basicProperties: properties,
//                     body: body);
channel.BasicPublish(exchange: "",
                     routingKey: QUEUE_NAME,
                     basicProperties: null,
                     body: body);

Console.WriteLine($" [x] Sent '{message}'");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();