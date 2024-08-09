// Publish/Subscribe Tutorial

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();

const string EXCHANGE_NAME = "fanout-exchange"; // Fanout exchange name

// Create channel1 for 'xiaowang_queue'
var channel1 = connection.CreateModel();
channel1.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Fanout);

var queueName1 = "xiaowang_queue";
channel1.QueueDeclare(queue: queueName1,
                      durable: true,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);
channel1.QueueBind(queue: queueName1,
                   exchange: EXCHANGE_NAME,
                   routingKey: string.Empty);

// Create channel2 for 'xiaoli_queue'
var channel2 = connection.CreateModel();
channel2.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Fanout);

var queueName2 = "xiaoli_queue";
channel2.QueueDeclare(queue: queueName2,
                      durable: true,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);
channel2.QueueBind(queue: queueName2,
                   exchange: EXCHANGE_NAME,
                   routingKey: string.Empty);

// Set up the consumer for channel1 ('xiaowang_queue')
var consumer1 = new EventingBasicConsumer(channel1);
consumer1.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [xiaowang] Received '{message}'");
};
channel1.BasicConsume(queue: queueName1,
                      autoAck: true,
                      consumer: consumer1);

// Set up the consumer for channel2 ('xiaoli_queue')
var consumer2 = new EventingBasicConsumer(channel2);
consumer2.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [xiaoli] Received '{message}'");
};
channel2.BasicConsume(queue: queueName2,
                      autoAck: true,
                      consumer: consumer2);

Console.WriteLine(" [*] Waiting for messages. To exit press [enter].");
Console.ReadLine();