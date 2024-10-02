using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp_Mini_Project.Models
{
    public class Notice
    {
        [Key]
        public int ID { get; set; }

        // ID Account
        public int UserID { get; set; }

        // Message
        [Required]
        public string Message { get; set; }

        // Photo
        public byte[]? Picture { get; set; }

        // IsRead สำหรับการแจ้งเตือนการอ่าน
        public bool IsRead { get; set; } = false;


        // คุณสมบัติการนำทาง (ไม่บังคับ) เพื่อเชื่อมโยงกับโมเดลบัญชีผู้ใช้
        [ForeignKey("UserID")]
        public virtual Account User { get; set; }

    }
}