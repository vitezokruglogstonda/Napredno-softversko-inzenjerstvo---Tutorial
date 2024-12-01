using System.ComponentModel.DataAnnotations;

namespace Tutorial.Models.Requests
{
    public class CreateItemRequest
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
