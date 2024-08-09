// Work Queues Tutorial

using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "multi_queue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

// keep reading the user input, then set as message
while(true)
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
    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "multi_queue",
                         basicProperties: properties,
                         body: body);

    Console.WriteLine($" [x] Sent {message}");
}

Console.WriteLine("Exited. Press [enter] to close the application.");
Console.ReadLine();


//var message = GetMessage(args);
//var body = Encoding.UTF8.GetBytes(message);

//var properties = channel.CreateBasicProperties();
//properties.Persistent = true;

//channel.BasicPublish(exchange: string.Empty,
//                     routingKey: "task_queue",
//                     basicProperties: properties,
//                     body: body);
//Console.WriteLine($" [x] Sent {message}");

//Console.WriteLine(" Press [enter] to exit.");
//Console.ReadLine();

//static string GetMessage(string[] args)
//{
//    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
//}