using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EsportMVC.Models
{
    public class User : IdentityUser
    {
        
        public int Year { get; set; }
    }
}