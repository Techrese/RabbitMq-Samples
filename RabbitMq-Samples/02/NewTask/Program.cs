using RabbitMQ.Client;
using System.Text;

const string Exchange = "logs";

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {       
        channel.ExchangeDeclare(Exchange,ExchangeType.Fanout);
        var message = GetMessage(args);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(Exchange, "", null, body);

        Console.WriteLine($"sent message {message}");
    }
}

Console.WriteLine("press enter to exit");
Console.ReadLine();



static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}