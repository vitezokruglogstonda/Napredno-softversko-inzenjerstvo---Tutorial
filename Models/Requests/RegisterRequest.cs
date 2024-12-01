using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Tutorial.Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
