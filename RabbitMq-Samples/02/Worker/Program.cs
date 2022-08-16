using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

const string Exchange = "logs";

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        var queueName = channel.QueueDeclare().QueueName;

        channel.ExchangeDeclare(Exchange, ExchangeType.Fanout);

        channel.QueueBind(queueName, Exchange, "");

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received: {message}");
                       
        };

        channel.BasicConsume(queueName, true, consumer);

        Console.WriteLine("press enter to exit");
        Console.ReadLine();
    }
}