using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

        // Declare the same fanout exchange
        channel.ExchangeDeclare(exchange: "school_announcement", type: "fanout");

        // Declare a queue for this consumer
        var queueName = channel.QueueDeclare().QueueName;

        // Bind the queue to the fanout exchange
        channel.QueueBind(queue: queueName,
                          exchange: "school_announcement",
                          routingKey: "");

        // Create a consumer to handle messages
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        };

        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine(" [*] Waiting for messages. Press [enter] to exit.");
        Console.ReadLine();
    }
}
