using System.ComponentModel.DataAnnotations;

namespace WebApp_Mini_Project.Models
{
    public class Chat
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
        [Required]
        public string User { get; set; } // ผู้ส่งข้อความ


    }
}
