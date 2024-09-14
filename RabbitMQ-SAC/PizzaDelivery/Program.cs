using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        ReceivePizzaOrder();
    }

    static void ReceivePizzaOrder()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "pizza_orders",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: new Dictionary<string, object> { { "x-queue-type", "quorum" } });

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("Received: {0}", message);
        };

        channel.BasicConsume(queue: "pizza_orders",
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine("Waiting for messages. Press [enter] to exit.");
        Console.ReadLine();
    }
}
