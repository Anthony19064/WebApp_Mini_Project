using System.ComponentModel.DataAnnotations;

namespace WebApp_Mini_Project.Models
{
    public class Chat
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public string User { get; set; } // ผู้ส่งข้อความ
        public byte[]? ProfilePicture { get; set; }


    }
}
