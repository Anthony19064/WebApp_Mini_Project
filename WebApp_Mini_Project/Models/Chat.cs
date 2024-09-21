namespace WebApp_Mini_Project.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }

        public string User { get; set; } // ผู้ส่งข้อความ


    }
}