using Microsoft.AspNetCore.Mvc;
using WebApp_Mini_Project.Models;

namespace WebApp_Mini_Project.Controllers
{
    public class AccountController : Controller
    {
        public static List<Account> accounts = new List<Account>
        {
            new Account("admin", "admin", "admin@gmail.com"),
        };

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string Username, string Password, string Email)
        {
            var newAccount = new Account(Username, Password, Email);
            accounts.Add(newAccount);
            return RedirectToAction("Index", "Post");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var user = accounts.SingleOrDefault(a => a.Username == Username && a.Password == Password);
            if (user != null)
            {
                // การเข้าสู่ระบบสำเร็จ: ตั้งค่า session หรือ cookie
                // ในกรณีนี้, เราจะเปลี่ยนเส้นทางไปที่หน้า Home
                return RedirectToAction("Index", "Post");
            }
            else
            {
                // การเข้าสู่ระบบล้มเหลว: แสดงข้อความข้อผิดพลาด
                @ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }
        }
    }
}
