// Topics Tutorial

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
const string EXCHANGE_NAME = "topic-exchange";

channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Topic);

// Declare the first queue and bind it with the routing key "frontend"
var queueName1 = "frontend_queue";
channel.QueueDeclare(queue: queueName1,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
channel.QueueBind(queue: queueName1,
                  exchange: EXCHANGE_NAME,
                  routingKey: "#.frontend.#");

// Declare the second queue and bind it with the routing key "backend"
var queueName2 = "backend_queue";
channel.QueueDeclare(queue: queueName2,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
channel.QueueBind(queue: queueName2,
                  exchange: EXCHANGE_NAME,
                  routingKey: "#.backend.#");


// Declare the second queue and bind it with the routing key "product"
var queueName3 = "product_queue";
channel.QueueDeclare(queue: queueName3,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
channel.QueueBind(queue: queueName3,
                  exchange: EXCHANGE_NAME,
                  routingKey: "#.product.#");


Console.WriteLine(" [*] Waiting for messages.");

// Set up the consumer for the "frontend_queue"
var consumer1 = new EventingBasicConsumer(channel);
consumer1.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($" [xiaoa] Received '{routingKey}':'{message}'");
};
channel.BasicConsume(queue: queueName1,
                     autoAck: true,
                     consumer: consumer1);

// Set up the consumer for the "backend_queue"
var consumer2 = new EventingBasicConsumer(channel);
consumer2.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($" [xiaob] Received '{routingKey}':'{message}'");
};
channel.BasicConsume(queue: queueName2,
                     autoAck: true,
                     consumer: consumer2);

// Set up the consumer for the "product_queue"
var consumer3 = new EventingBasicConsumer(channel);
consumer3.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;
    Console.WriteLine($" [xiaoc] Received '{routingKey}':'{message}'");
};
channel.BasicConsume(queue: queueName3,
                     autoAck: true,
                     consumer: consumer3);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();