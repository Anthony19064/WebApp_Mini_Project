using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
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
            var usersession = HttpContext.Session.GetString("Usersession");
            if (usersession == null)
            {
                return Unauthorized();
            }

            var account = await _db.Accounts.SingleOrDefaultAsync(account => account.Username == usersession);
            if (account != null)
            {
                message.ProfilePicture = account.ProfilePicture;
            }

            message.CreatedAt = DateTime.Now;
            _db.Chats.Add(message);
            await _db.SaveChangesAsync(); // ควรใช้ await
            return Ok();
        }


        // รับข้อความทั้งหมด
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _db.Chats
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => new {
                    m.ID,
                    m.Message,
                    m.CreatedAt,
                    m.User,
                    m.ProfilePicture // ตรวจสอบว่าถูกดึงมา
                })
                .ToListAsync();

            foreach (var message in messages)
            {
                if (message.ProfilePicture == null)
                {
                    Debug.WriteLine($"Message ID {message.ID} has a null ProfilePicture.");
                }
                else
                {
                    Debug.WriteLine($"Message ID {message.ID} has a ProfilePicture of size: {message.ProfilePicture.Length} bytes.");
                }
            }

            return Json(messages);
        }


    }
}