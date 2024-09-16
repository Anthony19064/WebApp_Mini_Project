using System.ComponentModel.DataAnnotations;

namespace WebApp_Mini_Project.Models
{
    public class Account
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        
       
    }
}
