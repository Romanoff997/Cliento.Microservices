namespace Cliento.Microservices.Shared.Events
{
    public record SessionPlannedEvent(Guid SessionId, Guid ClientId, DateTime ScheduledAt, int DurationInMinutes);
}
