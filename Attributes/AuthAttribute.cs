using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tutorial.Models;
using Tutorial.Models.Database;

namespace Tutorial.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthAttribute : Attribute, IAuthorizationFilter
    {
        public List<UserType>? Roles { get; set; }  
        public AuthAttribute() 
        {
            this.Roles = null;
        }

        public AuthAttribute(string[] roles) 
        {
            this.Roles = new List<UserType>();

            foreach (string role in roles)
            {
                UserType value;
                if (Enum.TryParse<UserType>(role, true, out value))
                {
                    this.Roles.Add(value);
                }
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            User? user = (User?)context.HttpContext.Items["User"];
            if (user == null || (this.Roles != null && !this.Roles.Contains(user.UserType)))
                context.Result = new UnauthorizedObjectResult("Invalid token.");   
        }
    }
}
