using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "rabbit-mq-service" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "message_queue", type: "direct");

Task.Run(() =>
{
    Thread.Sleep(1000);
    var seq = 0;
    channel.BasicPublish(
        exchange: "message_queue",
        routingKey: "standard",
        basicProperties: null,
        body: Encoding.UTF8.GetBytes($"count: {seq}"));

    Console.WriteLine("inserted: {0}, next: {1}", seq, seq++);
});
