using static System.Collections.Specialized.BitVector32;

namespace Cliento.Microservices.CRMService.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Session> Sessions { get; set; } = new();
    }
}
