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
            var userSession = HttpContext.Session.GetString("Usersession");
            if (userSession == null)
            {
                // ส่งกลับ 401 Unauthorized เมื่อไม่ล็อกอิน
                return Unauthorized();
            }

            // ตรวจสอบว่าข้อความว่างไหม
            if (string.IsNullOrWhiteSpace(message.Message))
            {
                // ส่งกลับ 400 BadRequest ถ้าข้อความว่าง
                return BadRequest();
            }

            message.CreatedAt = DateTime.Now;

            _db.Chats.Add(message);
            await _db.SaveChangesAsync(); // ใช้ await เพื่อให้การบันทึกข้อมูลเป็น asynchronous

            return Ok(); // ส่งผลลัพธ์ OK ถ้าสำเร็จ
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