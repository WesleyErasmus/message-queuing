using System.Text;
using RabbitMQ.Client;


// Class used for connecting to RabbitMQ
public class RabbitMQConnector
{
    // Private property for HostName
    private string _hostName = "localhost";
    // Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/using-properties

    // Method for connection to send messages to the queue
    public void Producer(string message)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "wnga-rabbit",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: "",
            routingKey: "wnga-rabbit",
            basicProperties: null,
            body: body);
    }
};

// Class used for creating and sending the message to the rabbitMQ queue
public class Program
{
    public static void Main(string[] args)
    {
        // Output line to the console allow user to enter their name
        Console.WriteLine("Enter your name:");
        string name = Console.ReadLine();

        string message = $"Hello my name is, {name}";

        // Creating a connection to rabbitMQ and sending the message using the Producer method in the RabbitMQConnector class
        RabbitMQConnector connector = new RabbitMQConnector();
        connector.Producer(message);

        Console.WriteLine($"[Message sent]: {message}");
    }
}
