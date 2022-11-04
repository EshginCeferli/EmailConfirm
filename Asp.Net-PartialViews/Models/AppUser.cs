using Microsoft.AspNetCore.Identity;

namespace Asp.Net_PartialViews.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
        public int Age { get; set; }
        public bool IsActivated { get; set; } = false;
    }
}
