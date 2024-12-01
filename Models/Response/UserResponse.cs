using Tutorial.Models.Database;

namespace Tutorial.Models.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public UserResponse(User user)
        {
            Id = user.Id;
            Email = user.Email;
            UserType = user.UserType;
        }
    }
}
