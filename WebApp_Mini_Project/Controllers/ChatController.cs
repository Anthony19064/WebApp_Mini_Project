using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApp_Mini_Project.Data;
using WebApp_Mini_Project.Models;

namespace WebApp_Mini_Project.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDBContext _db;

        public ChatController(ApplicationDBContext db)
        {
            _db = db;
        }

        [HttpPost]
        public ActionResult AddChat(Chat obj) // ใช้ ActionResult แทน void
        {
            string usersession = HttpContext.Session.GetString("Usersession");
            if (usersession == null)
            {
                // แทนที่จะใช้ return ที่ไม่มีค่า ให้ส่งกลับ NotFound
                return RedirectToAction("Index", "Post");
            }

            var account = _db.Accounts.SingleOrDefault(a => a.Username == usersession);
            if (account == null)
            {
                return NotFound(); // ไม่ต้องทำอะไรต่อ ถ้าไม่พบบัญชีผู้ใช้
            }

            if (string.IsNullOrWhiteSpace(obj.Message))
            {
                return BadRequest("Message cannot be empty."); // ส่งกลับข้อผิดพลาดถ้าข้อความว่างเปล่า
            }

            obj.ProfilePicture = account.ProfilePicture;
            obj.CreatedAt = DateTime.UtcNow;

            _db.Chats.Add(obj);
            _db.SaveChanges();

            // ส่งกลับเป็น NoContent ถ้าทำงานสำเร็จและไม่ต้องการคืนค่าใด ๆ
            return NoContent(); // หรือสามารถใช้ Ok() ถ้าต้องการส่งข้อมูลเพิ่มเติม
        }

        public ActionResult GetChats()
        {
            var chats = _db.Chats.OrderByDescending(m => m.CreatedAt)
                .Take(7)
                .Select(chat => new {
                    chat.User,
                    chat.Message,
                    CreatedAt = chat.CreatedAt.ToString("o"), // ส่งในรูปแบบ ISO 8601
                    ProfilePicture = chat.ProfilePicture
                })
                .ToList();

            return Json(chats);
        }


    }
}