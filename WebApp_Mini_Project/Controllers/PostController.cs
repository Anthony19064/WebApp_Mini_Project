using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApp_Mini_Project.Controllers;
using WebApp_Mini_Project.Data;
using WebApp_Mini_Project.Models;
using WebApp_Mini_Project.ViewModels;

public class PostController : HomeController
{
    public readonly ApplicationDBContext _db;

    public PostController(ApplicationDBContext db)
    {
        _db = db;
    }
    public ActionResult Index()
    {
        var defaultImagePath = "wwwroot/Image/Logo_web.png"; // ระบุ path ของรูปภาพเริ่มต้น
        var adminAccount = new Account
        {
            Username = "admin",
            Password = "admin",
            Email = "admin@gmail.com",
            ProfilePicture = System.IO.File.ReadAllBytes(defaultImagePath)
        };

        var check_admin = _db.Accounts.SingleOrDefault(a => a.Username == adminAccount.Username && a.Password == adminAccount.Password && a.Email == adminAccount.Email);
        if (check_admin == null)
        {
            _db.Accounts.Add(adminAccount);
            _db.SaveChanges();
        }


        string usersession = HttpContext.Session.GetString("Usersession");
        if (usersession != null)
        {
            var account = _db.Accounts.SingleOrDefault(account => account.Username == usersession);
            int userid = account.ID;

            ViewBag.UserID = userid;
        }
        var chat = _db.Chats.OrderByDescending(m => m.CreatedAt).ToList();
        var accounts = _db.Accounts.ToList();
        var posts = _db.Posts.ToList(); // ข้อมูลใน DB เก็บลง ตัวแปร posts
        posts.Reverse();
        var newPost = new Post(); // ข้อมูลที่รับมาจากฟอรม์ เก็บลง ตัวแปร newPost


        var viewModel = new PostViewModel // เอาสองตัวบนมาเก็บลง object ViewModel
        {
            Posts = posts,
            NewPost = newPost,
            Account = accounts,
            Chats = chat,
        };
        ViewBag.Usersession = usersession; // ViewBag ส่งค่า session ที่เก็บไว้ออกไป


        return View(viewModel); // ส่ง ViewModel ไปยัง View index
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(PostViewModel model) // สร้างตัวแปร PostViewModel ที่รับค่าจากสองตัวเมื่อกี้ มาเก็บลงตัวแปร model
    {
        string usersession = HttpContext.Session.GetString("Usersession");
        var account = _db.Accounts.SingleOrDefault(account => account.Username == usersession);
        model.NewPost.User_id = account.ID;
        model.NewPost.timeCreate = DateTime.Now;

        DateTime timeEnd = model.NewPost.dayend + model.NewPost.timeend; // รวม DateTime

        model.NewPost.timeout = timeEnd;

        _db.Posts.Add(model.NewPost); // เอาเฉพาะ Newpost มาเก็บลง DB
        _db.SaveChanges(); // บันทึกข้อมูล
        return RedirectToAction("Index"); // กลับไปหน้า Index

    }

    [HttpGet]
    public IActionResult GetNotifications()
    {
        // ตรวจสอบการล็อกอินของผู้ใช้
        string usersession = HttpContext.Session.GetString("Usersession");
        if (string.IsNullOrEmpty(usersession))
        {
            return Json(new { success = false, message = "คุณยังไม่ได้ล็อกอิน" });
        }

        // ดึงข้อมูลผู้ใช้จากฐานข้อมูล
        var account = _db.Accounts.SingleOrDefault(a => a.Username == usersession);
        if (account == null)
        {
            return Json(new { success = false, message = "ไม่พบข้อมูลผู้ใช้" });
        }

        // ดึงข้อมูลการแจ้งเตือนเฉพาะของผู้ใช้ที่ล็อกอินอยู่
        var notifications = _db.Notices
            .Where(n => n.UserID == account.ID) // ดึงการแจ้งเตือนของผู้ใช้ที่ล็อกอิน
            .Select(n => new
            {
                senderUsername = _db.Accounts.FirstOrDefault(a => a.ID == n.UserID).Username, // ผู้ส่ง
                message = n.Message, // ข้อความแจ้งเตือน
                picture = n.Picture != null ? Convert.ToBase64String(n.Picture) : null // รูปภาพ (แปลงเป็น Base64)
            })
            .ToList();
        var hasnewnotifications = _db.Notices
            .Where(n => n.UserID == account.ID && !n.IsRead) // ดึงการแจ้งเตือนของผู้ใช้ที่ล็อกอิน
            .Select(n => new
            {
                senderUsername = _db.Accounts.FirstOrDefault(a => a.ID == n.UserID).Username, // ผู้ส่ง
                message = n.Message, // ข้อความแจ้งเตือน
                picture = n.Picture != null ? Convert.ToBase64String(n.Picture) : null // รูปภาพ (แปลงเป็น Base64)
            })
            .ToList();
        bool newNotification = hasnewnotifications.Any();
        // ส่งข้อมูลกลับในรูปแบบ JSON
        return Json(new { success = true, notifications, newNotification });
    }








    [HttpPost]
    public IActionResult Joinroom(int? id, string user)
    {
        // ตรวจสอบสถานะการล็อกอิน
        string usersession = HttpContext.Session.GetString("Usersession");
        if (string.IsNullOrEmpty(usersession))
        {
            return Json(new { success = false, message = "คุณยังไม่ได้ล็อกอิน" });
        }

        var account = _db.Accounts.SingleOrDefault(account => account.Username == usersession);

        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _db.Posts.Find(id);
        if (obj == null)
        {
            return NotFound(); // ถ้าไม่พบโพสต์
        }

        // เช็คว่าผู้ใช้อยู่ในรายการหรือไม่
        if (obj.User_list.Contains(account.ID))
        {
            return Json(new { success = false, message = "คุณได้เข้าร่วมห้องนี้แล้ว." });
        }
        else
        {
            var own_post = _db.Accounts.Find(obj.User_id);
            // อัพเดทข้อมูล
            obj.Count_person++; // เพิ่มจำนวนผู้เข้าร่วม
            obj.User_list.Add(account.ID); // เพิ่มผู้ใช้ในรายการ
            if (obj.Count_person == obj.Max_person)
            {
                obj.status = false;
            }

            // สร้างการแจ้งเตือน
            var notice = new Notice
            {
                UserID = account.ID,
                Message = $"คุณได้เข้าร่วม เลขห้อง : {obj.Id_room}",
                Picture = own_post.ProfilePicture // ต้องมีค่าจริงในที่นี้
            };




            // บันทึกการแจ้งเตือนลงในฐานข้อมูล
            _db.Notices.Add(notice);
        }

        _db.SaveChanges();

        // คืนค่าตอบกลับ JSON ที่มีข้อมูลใหม่ รวมถึง Id_room
        return Json(new
        {
            success = true,
            message = $"คุณได้เข้าร่วม เลขห้อง : {obj.Id_room}", // ปรับข้อความที่แสดงเลขห้อง
            newCount = obj.Count_person,
            roomId = obj.Id_room  // ส่งค่า Id_room กลับไปด้วย
        });
    }




    [HttpPost]
    public IActionResult EditPost(int id, string roomname, string roomid, string game, string detail, int count, string day, string time)
    {
        var post = _db.Posts.SingleOrDefault(a => a.ID == id);
        if (post != null)
        {
            post.Name_room = roomname;
            post.Id_room = roomid;
            post.Game = game;
            post.Details = detail;
            post.Max_person = count;
            DateTime timeend = DateTime.TryParse($"{day} {time}", out DateTime parsedTime) ? parsedTime : DateTime.MinValue;
            post.timeout = timeend;
            _db.SaveChanges();
        }
        else
        {
            return NotFound();
        }
        return RedirectToAction("Profile", "Account");
    }

    [HttpPost]
    public IActionResult DeletePost(int id)
    {
        var post = _db.Posts.SingleOrDefault(a => a.ID == id);
        if (post != null)
        {
            _db.Posts.Remove(post);
            _db.SaveChanges();
            return RedirectToAction("Profile", "Account");
        }
        else
        {
            return NotFound();
        }

    }

    public IActionResult ClosePost(int id)
    {
        var post = _db.Posts.SingleOrDefault(a => a.ID == id);
        if (post != null)
        {
            post.status = false;
            _db.SaveChanges();
            return RedirectToAction("Profile", "Account");
        }
        else
        {
            return NotFound();
        }
    }
    [HttpPost]
    public IActionResult MarkAllNotificationsAsRead()
    {
        // Check user login status using session data
        string usersession = HttpContext.Session.GetString("Usersession");
        if (string.IsNullOrEmpty(usersession))
        {
            return Json(new { success = false, message = "คุณยังไม่ได้ล็อกอิน" });
        }

        // Retrieve the logged-in user's account from the database
        var account = _db.Accounts.SingleOrDefault(a => a.Username == usersession);
        if (account == null)
        {
            return Json(new { success = false, message = "ไม่พบข้อมูลผู้ใช้" });
        }

        // Get all unread notifications for this user and mark them as read
        var unreadNotifications = _db.Notices
            .Where(n => n.UserID == account.ID && !n.IsRead)
            .ToList();

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true; // Mark each notification as read
        }

        // Save changes to the database
        _db.SaveChanges();

        // Return success response
        return Json(new { success = true, message = "การแจ้งเตือนทั้งหมดถูกอ่านแล้ว" });
    }






}