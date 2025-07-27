using System.ComponentModel.DataAnnotations;

namespace AuthenticationFlow.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;
    }
}
