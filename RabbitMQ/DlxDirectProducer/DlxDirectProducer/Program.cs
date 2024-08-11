using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
const string DEAD_EXCHANGE_NAME = "dlx-direct-exchange";
const string WORK_EXCHANGE_NAME = "direct2-exchange";

channel.ExchangeDeclare(exchange: DEAD_EXCHANGE_NAME, type: ExchangeType.Direct);

// Declare the DLX queues and bind them to the DLX exchange with routing keys
var queueName1 = "laoban_dlx_queue";
channel.QueueDeclare(queue: queueName1, durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.QueueBind(queue: queueName1, exchange: DEAD_EXCHANGE_NAME, routingKey: "laoban");

var queueName2 = "waibao_dlx_queue";
channel.QueueDeclare(queue: queueName2, durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.QueueBind(queue: queueName2, exchange: DEAD_EXCHANGE_NAME, routingKey: "waibao");

// Set up the consumer for the "laoban_dlx_queue"
var laobanConsumer = new EventingBasicConsumer(channel);
laobanConsumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [laoban] Received '{ea.RoutingKey}':'{message}'");

    // Reject the message and do not requeue it
    channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
};
channel.BasicConsume(queue: queueName1, autoAck: false, consumer: laobanConsumer);

// Set up the consumer for the "waibao_dlx_queue"
var waibaoConsumer = new EventingBasicConsumer(channel);
waibaoConsumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [waibao] Received '{ea.RoutingKey}':'{message}'");

    // Reject the message and do not requeue it
    channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
};
channel.BasicConsume(queue: queueName2, autoAck: false, consumer: waibaoConsumer);




Console.WriteLine("Enter messages in the format: [message] [routingKey]");
Console.WriteLine("Type 'exit' to quit.");

while (true)
{
    // Read user input
    var userInput = Console.ReadLine();
    if (userInput == null || userInput.ToLower() == "exit")
        break;

    // Split the input to get the message and routing key
    var inputParts = userInput.Split(' ');
    if (inputParts.Length < 2)
    {
        Console.WriteLine("Please enter a message followed by a routing key.");
        continue;
    }

    var message = inputParts[0];
    var routingKey = inputParts[1];

    // Convert the message to a byte array
    var body = Encoding.UTF8.GetBytes(message);

    // Publish the message to the exchange with the specified routing key
    channel.BasicPublish(exchange: WORK_EXCHANGE_NAME,
                         routingKey: routingKey,
                         basicProperties: null,
                         body: body);
    Console.WriteLine($" [x] Sent '{message}' with routing key '{routingKey}'");
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();