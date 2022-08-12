using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare("task_queue", true, false, false, null);        

        var message = GetMessage(args);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish("", "task_queue", null, body);

        Console.WriteLine($"sent message {message}");
    }
}

Console.WriteLine("press enter to exit");
Console.ReadLine();



static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}