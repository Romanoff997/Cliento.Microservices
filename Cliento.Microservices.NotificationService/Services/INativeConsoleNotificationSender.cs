namespace Cliento.Microservices.NotificationService.Services
{
    public interface INativeConsoleNotificationSender
    {
        public Task Send(string body, string routingKey);
    }
}
