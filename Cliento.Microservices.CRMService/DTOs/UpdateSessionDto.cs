using Cliento.Microservices.CRMService.Models;

namespace Cliento.Microservices.CRMService.DTOs
{
    public class UpdateSessionDto
    {
        public DateTime ScheduledAt { get; set; }
        public int DurationInMinutes { get; set; }
        public string Notes { get; set; } = string.Empty;
        public SessionStatus Status { get; set; } = SessionStatus.Planned;
    }
}
