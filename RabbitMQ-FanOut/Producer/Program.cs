using RabbitMQ.Client;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Create a connection to RabbitMQ
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Declare a fanout exchange
        channel.ExchangeDeclare(exchange: "school_announcement", type: "fanout");

        // Create a message
        string message = "The school play is tomorrow!";
        var body = Encoding.UTF8.GetBytes(message);

        // Publish the message to the fanout exchange
        channel.BasicPublish(exchange: "school_announcement",
                             routingKey: "",
                             basicProperties: null,
                             body: body);

        Console.WriteLine(" [x] Sent {0}", message);
    }
}
