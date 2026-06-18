namespace Cliento.Microservices.CRMService.Models
{
    public class UserCrm
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
