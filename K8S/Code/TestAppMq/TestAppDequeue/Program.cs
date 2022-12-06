using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "rabbit-mq-service" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "message_queue", type: "direct");
var queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(
     queue: queueName,
     exchange: "message_queue",
     routingKey: "standard");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    if (ea.RoutingKey != "standard")
        return;
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine("received: {0}", message);
};

Task.Run(() =>
{
    Thread.Sleep(2000);
    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
});
