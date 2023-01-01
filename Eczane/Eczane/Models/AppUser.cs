using Microsoft.AspNetCore.Identity;

namespace Eczane.Models
{
    public class AppUser:IdentityUser
    {

        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
    }
}
