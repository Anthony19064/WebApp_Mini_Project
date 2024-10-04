using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using WebApp_Mini_Project.Data;
using WebApp_Mini_Project.Models;
using WebApp_Mini_Project.ViewModels;

namespace WebApp_Mini_Project.Controllers
{
    public class AccountController : HomeController
    {
        public readonly ApplicationDBContext _db;

        public AccountController(ApplicationDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Account obj, IFormFile? profilePicture)
        {
            if (ModelState.IsValid)
            {
                bool usernameExists = _db.Accounts.Any(a => a.Username == obj.Username);
                bool emailExists = _db.Accounts.Any(b => b.Email == obj.Email);

                if (usernameExists)
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                }
                else if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                }
                else
                {
                    if (obj.Password == obj.ReplyPassword)
                    {
                        if(profilePicture == null)
                        {
                            var defaultImagePath = "wwwroot/Image/User_icon.png"; // ระบุ path ของรูปภาพเริ่มต้น
                            obj.ProfilePicture = System.IO.File.ReadAllBytes(defaultImagePath);
                        }
                        // Handle the image file
                        else if (profilePicture != null && profilePicture.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await profilePicture.CopyToAsync(memoryStream);
                                obj.ProfilePicture = memoryStream.ToArray();
                            }
                        }
                        


                        // Save the account to the database
                        _db.Accounts.Add(obj);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("ReplyPassword", "Reply password does not match the password.");
                    }
                }
            }
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

            var user = _db.Accounts.SingleOrDefault(a => a.Username == username);

            if (user != null && user.Password == password)
            {
                HttpContext.Session.SetString("Usersession", user.Username);
                return RedirectToAction("Index", "Post");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Post");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var posts = _db.Posts.ToList();
            var accounts = _db.Accounts.ToList();
            string usersession = HttpContext.Session.GetString("Usersession");
            var account = _db.Accounts.SingleOrDefault(account => account.Username == usersession);
            if (accounts == null)
            {
                return NotFound(); 
            }
                
            var viewModel = new AccountViewModel
            {
                Posts = posts,
                Account = account,
                Account_list = accounts
            };
            int userid = account.ID;
            ViewBag.UserID = userid;
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditProfile(int id, string username, string password, string email, IFormFile profilePicture)
        {
            var account = _db.Accounts.SingleOrDefault(a => a.ID == id);
            if (account != null)
            {
                account.Username = username;

                if (!string.IsNullOrEmpty(password))
                {
                    account.Password = password; // ควรเข้ารหัสด้วย
                }

                account.Email = email;

                // การจัดการการอัปโหลดรูปภาพ
                if (profilePicture != null && profilePicture.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        profilePicture.CopyTo(memoryStream);
                        account.ProfilePicture = memoryStream.ToArray();
                    }
                }
                HttpContext.Session.SetString("Usersession", username);
                _db.SaveChanges();

            }

            return RedirectToAction("Profile", "Account");
        }

    }
}
