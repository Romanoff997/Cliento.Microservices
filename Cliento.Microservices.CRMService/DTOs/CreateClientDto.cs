namespace Cliento.Microservices.CRMService.DTOs
{
    public class CreateClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
    }
}
