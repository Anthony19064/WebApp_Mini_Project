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

        // แสดงหน้าแชท
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Usersession = HttpContext.Session.GetString("Usersession");
            return View();
        }

        // ส่งข้อความ
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] Chat message)
        {
            // ตรวจสอบว่าเข้าสู่ระบบหรือยัง
            var userSeccion = HttpContext.Session.GetString("Usersession");
            if (string.IsNullOrEmpty(userSeccion))
            {
                return Unauthorized(); // Return หน้า 401 Unauthorized
            }

            message.CreatedAt = DateTime.Now;

            _db.Chats.Add(message);
            await _db.SaveChangesAsync();
            return Ok();
        }

        // รับข้อความทั้งหมด
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _db.Chats
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return Json(messages);
        }
    }
}