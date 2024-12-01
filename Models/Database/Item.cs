using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Tutorial.Models.Requests;

namespace Tutorial.Models.Database
{
    [Table("Item")]
    public class Item
    {
        [Key]
        [Required]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("Description")]
        [DataType(DataType.Text)]
        public string Description { get; set; } = string.Empty;

        public int OwnerId { get; set; }

        [Required]
        [JsonIgnore]
        public User? Owner { get; set; }

        public Item(CreateItemRequest itemRequest, int userId)
        {
            Title = itemRequest.Title!;
            Description = itemRequest.Description!;
            OwnerId = userId;
        }
    }
}
