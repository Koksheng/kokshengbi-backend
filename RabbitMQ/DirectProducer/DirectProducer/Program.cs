using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
const string EXCHANGE_NAME = "direct-exchange";

channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Direct);

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
    channel.BasicPublish(exchange: EXCHANGE_NAME,
                         routingKey: routingKey,
                         basicProperties: null,
                         body: body);
    Console.WriteLine($" [x] Sent '{message}' with routing key '{routingKey}'");
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();