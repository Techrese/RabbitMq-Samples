using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare("hello", false, false, false, null);

        string message = "Hello World";

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", "hello", null, body);

        Console.WriteLine($"sent message {message}");
    }
}

Console.WriteLine("press enter to exit");
Console.ReadLine();