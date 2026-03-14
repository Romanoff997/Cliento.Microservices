using Cliento.Microservices.Shared.Events;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Cliento.Microservices.NotificationService.Consumers
{

    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchange = "crm.events";
        private readonly string _queueName = "crm.events.queue";

        public RabbitMqConsumer(string rabbitHost)
        {
            var factory = new ConnectionFactory { HostName = rabbitHost };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_exchange, ExchangeType.Topic, durable: true);
            _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(_queueName, _exchange, "client.created");
            _channel.QueueBind(_queueName, _exchange, "session.planned");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var routingKey = ea.RoutingKey;

                if (routingKey == "client.created")
                {
                    var evt = JsonSerializer.Deserialize<ClientCreatedEvent>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Console.WriteLine($"[Notification] Welcome email to {evt?.Email} for client {evt?.Name} ({evt?.ClientId})");
                }
                else if (routingKey == "session.planned")
                {
                    var evt = JsonSerializer.Deserialize<SessionPlannedEvent>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Console.WriteLine($"[Notification] Reminder set for session {evt?.SessionId} at {evt?.ScheduledAt} for client {evt?.ClientId}");
                }
                _channel.BasicAck(ea.DeliveryTag, multiple: false);
                await Task.Yield();
            };

            _channel.BasicConsume(_queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
