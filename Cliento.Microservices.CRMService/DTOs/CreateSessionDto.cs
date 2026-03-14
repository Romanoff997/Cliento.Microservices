namespace Cliento.Microservices.CRMService.DTOs
{
    public class CreateSessionDto
    {
        public DateTime ScheduledAt { get; set; }
        public int DurationInMinutes { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
