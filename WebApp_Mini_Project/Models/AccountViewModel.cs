using Microsoft.AspNetCore.Http;

namespace WebApp_Mini_Project.Models
{
    public class AccountViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IFormFile ProfilePictureFile { get; set; }
    }
}
