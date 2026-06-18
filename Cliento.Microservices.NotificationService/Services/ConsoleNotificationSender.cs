using Cliento.Microservices.Shared.Events;
using System.Text.Json;

namespace Cliento.Microservices.NotificationService.Services
{

    public class ConsoleNotificationSender: INativeConsoleNotificationSender
    {
        public Task Send(string body, string routingKey)
        {
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

            return Task.CompletedTask;
        }

    }
}
