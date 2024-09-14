using RabbitMQ.Client;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        SendPizzaOrder();
    }

    static void SendPizzaOrder()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();


        channel.QueueDeclare(queue: "pizza_orders",
                             durable: true,  
                             exclusive: false,
                             autoDelete: false,
                             arguments: new Dictionary<string, object> { { "x-queue-type", "quorum" } });

        string message = "Order: Pizza Margherita";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: "pizza_orders",
                             basicProperties: null,
                             body: body);

        Console.WriteLine("Sent: {0}", message);
    }
}
