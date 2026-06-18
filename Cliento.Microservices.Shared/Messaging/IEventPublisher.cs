namespace Cliento.Microservices.Shared.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAsync(string routingKey, object @event);
    }
}
