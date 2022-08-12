using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace Hello_World_Receive
{
    internal class Receive
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"Received: {message}");
                    };

                    channel.BasicConsume("hello", true, consumer);

                    Console.WriteLine("press enter to exit");
                    Console.ReadLine();
                }
            }
        }
    }
}
