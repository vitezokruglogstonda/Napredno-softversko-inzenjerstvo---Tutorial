using System.ComponentModel.DataAnnotations;

namespace Tutorial.Models.Requests
{
    public class LogInRequest
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
