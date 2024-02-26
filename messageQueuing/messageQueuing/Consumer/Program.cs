using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// Class used for connecting to RabbitMQ
public class RabbitMQConnector
{
    // Private property for HostName
    private string _hostName = "localhost";
    // Source: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/using-properties

    // Method to consume messages from the queue
    public void Consumer()
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

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            // Extracting name from message
            int startOfNameIndex = 18;
            var receivedName = message.Substring(startOfNameIndex);
            /* Substring Method Source: https://learn.microsoft.com/en-us/dotnet/api/system.string.substring?view=net-8.0 */

            // Validation to check if a name was entered or left empty
            if (receivedName.Length == 0)
            {
                Console.WriteLine("[Message error]: Error. No name was inputted.");
            }
            // Automatic response incoming messages
            Console.WriteLine($"[Message received]: {message}");
            Console.WriteLine($"[Automatic response]: Hello {receivedName}, I am your father!");
        };

        channel.BasicConsume(
            queue: "wnga-rabbit",
            autoAck: true,
            consumer: consumer
        );
    }
}

// Program class used to connect to RabbitMQ
public class Program
{
    public static void Main(string[] args)
    {
        RabbitMQConnector connector = new RabbitMQConnector();
        connector.Consumer();
    }
}