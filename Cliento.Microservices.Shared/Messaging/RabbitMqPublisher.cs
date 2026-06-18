using Cliento.Microservices.Shared.Settings;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Cliento.Microservices.Shared.Messaging
{
    public class RabbitMqPublisher : IEventPublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeName;

        public RabbitMqPublisher(RabbitMqSettings settings)
        {
            _exchangeName = settings.Exchange;
            var factory = new ConnectionFactory 
            { 
                HostName = settings.HostName,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password,
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Topic, durable: true);
        }

        public Task PublishAsync(string routingKey, object @event)
        {
            var json = JsonSerializer.Serialize(@event, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var body = Encoding.UTF8.GetBytes(json);
            var props = _channel.CreateBasicProperties();
            props.Persistent = true;
            _channel.BasicPublish(exchange: _exchangeName, routingKey: routingKey, basicProperties: props, body: body);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}


