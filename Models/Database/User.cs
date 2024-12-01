using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;
using System.Text.Json.Serialization;

namespace Tutorial.Models.Database
{
    [Table("User")]
    public class User
    {
        [Key]
        [Required]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Column("Password Hash")]
        public byte[] PasswordHash { get; set; } = [];

        [Required]
        public byte[]? PasswordSalt { get; set; }

        [Required]
        [Column("Role")]
        public UserType UserType { get; set; }

        [Column("Online")]
        public bool OnlineStatus { get; set; }
        public virtual List<Item>? Items { get; set; } = new();

        public User(string Email, byte[] PasswordHash, byte[]? PasswordSalt, UserType UserType, bool OnlineStatus)
        {
            this.Email = Email;
            this.PasswordHash = PasswordHash;
            this.PasswordSalt = PasswordSalt;
            this.UserType = UserType;
            this.OnlineStatus = OnlineStatus;
        }

        [JsonConstructor]
        public User(int Id, string Email, byte[] PasswordHash, byte[]? PasswordSalt, UserType UserType, bool OnlineStatus, List<Item>? Items)
        {
            this.Id = Id;
            this.Email = Email;
            this.PasswordHash = PasswordHash;
            this.PasswordSalt = PasswordSalt;
            this.UserType = UserType;
            this.OnlineStatus = OnlineStatus;
            this.Items = Items;
        }
    }
}
