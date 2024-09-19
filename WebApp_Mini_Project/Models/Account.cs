using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public string ReplyPassword { get; set; }

        [Required]
        public string Email { get; set; }

        public byte[]? ProfilePicture { get; set; }  

    }
}
