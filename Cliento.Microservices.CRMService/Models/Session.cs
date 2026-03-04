namespace Cliento.Microservices.CRMService.Models
{
    public enum SessionStatus { Planned, Completed, Cancelled }

    public class Session
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationInMinutes { get; set; }
        public SessionStatus Status { get; set; } = SessionStatus.Planned;
        public string Notes { get; set; } = string.Empty;
    }
}
