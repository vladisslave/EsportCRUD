using Microsoft.AspNetCore.Identity;

namespace EsportMVC.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}