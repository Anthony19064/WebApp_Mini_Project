using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp_Mini_Project.Data;
using WebApp_Mini_Project.Models;

namespace WebApp_Mini_Project.Controllers
{
    public class AccountController : Controller
    {
        public readonly ApplicationDBContext _db;

        public AccountController(ApplicationDBContext db)
        {
            _db = db
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Account obj)
        {
            // ตรวจสอบว่า ModelState ถูกต้องหรือไม่
            if (ModelState.IsValid)
            {
                // เพิ่มข้อมูลผู้ใช้ลงในฐานข้อมูล
                _db.Accounts.Add(obj);
                _db.SaveChanges();

                // เปลี่ยนเส้นทางไปยังหน้าหรือการกระทำอื่นๆ ตามที่ต้องการ
                return RedirectToAction("Index", "Post"); // หรือหน้าลงทะเบียนสำเร็จ
            }
            // หากข้อมูลไม่ถูกต้อง ให้คืนค่ากลับไปที่ฟอร์มและแสดงข้อผิดพลาด
            return View(obj);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Please enter both username and password.");
                return View();
            }
            // ตรวจสอบข้อมูลผู้ใช้จากฐานข้อมูล
            var user = _db.Accounts.SingleOrDefault(a => a.Username == username);
            // การเข้าสู่ระบบสำเร็จ
            if (user != null && user.Password == password)
            {
                // ตั้งค่า session 
                HttpContext.Session.SetString("Usersession", user.Username);
                // เปลี่ยนเส้นทางไปยังหน้าหรือการกระทำอื่นๆ ตามที่ต้องการ
                return RedirectToAction("Index", "Post");
            }
            // หากข้อมูลไม่ถูกต้อง
            ModelState.AddModelError("", "Invalid username or password");
            // คืนค่ากลับไปที่ฟอร์มและแสดงข้อผิดพลาด
            return View();
        }




    }
}
