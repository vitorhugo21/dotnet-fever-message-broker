using Send.Models;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/post-message", (Message message) =>
{
    if (string.IsNullOrEmpty(message.Content)) return Results.BadRequest();

    var factory = new ConnectionFactory() { HostName = "localhost" };
    using(var connection = factory.CreateConnection())
    using(var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "message-broker-tutorial", durable: false, exclusive: false, autoDelete: false, arguments: null);
        var body = Encoding.UTF8.GetBytes(message.Content);
        channel.BasicPublish(exchange: "", routingKey: "message-broker-tutorial", basicProperties: null, body: body);
    }

    return Results.Ok("Message Sent!");
});

app.Run();
