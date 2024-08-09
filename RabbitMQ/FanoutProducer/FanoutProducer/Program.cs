// Publish/Subscribe Tutorial

using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "fanout-exchange", type: ExchangeType.Fanout);

while (true)
{
    // Read user input
    var message = Console.ReadLine();

    // Check if the user wants to exit
    if (message.ToLower() == "exit")
        break;

    // Convert the message to byte array
    var body = Encoding.UTF8.GetBytes(message);

    var properties = channel.CreateBasicProperties();
    properties.Persistent = true;

    // Publish the message to the queue
    channel.BasicPublish(exchange: "fanout-exchange",
                     routingKey: string.Empty,
                     basicProperties: null,
                     body: body);

    Console.WriteLine($" [x] Sent {message}");
}

Console.WriteLine("Exited. Press [enter] to close the application.");
Console.ReadLine();

//var message = GetMessage(args);
//var body = Encoding.UTF8.GetBytes(message);
//channel.BasicPublish(exchange: "fanout-exchange",
//                     routingKey: string.Empty,
//                     basicProperties: null,
//                     body: body);
//Console.WriteLine($" [x] Sent {message}");

//Console.WriteLine(" Press [enter] to exit.");
//Console.ReadLine();

//static string GetMessage(string[] args)
//{
//    return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
//}