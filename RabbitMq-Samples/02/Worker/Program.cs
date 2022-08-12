using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare("task_queue", false, false, false, null);

        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received: {message}");

            int dots = message.Split('.').Length - 1;
            Thread.Sleep(dots * 1000);

            Console.WriteLine("done");

            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume("task_queue", false, consumer);

        Console.WriteLine("press enter to exit");
        Console.ReadLine();
    }
}