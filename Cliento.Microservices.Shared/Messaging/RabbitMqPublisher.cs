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

        public RabbitMqPublisher(string hostName, string exchangeName = "crm.events")
        {
            _exchangeName = exchangeName;
            var factory = new ConnectionFactory { HostName = hostName };
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


