namespace Cliento.Microservices.Shared.Events
{
    public record ClientCreatedEvent(Guid ClientId, string Name, string Email);
}
