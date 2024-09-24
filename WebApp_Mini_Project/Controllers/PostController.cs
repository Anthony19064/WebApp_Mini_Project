using Microsoft.AspNetCore.Mvc;
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
        var adminAccount = new Account
        {
            Username = "admin",
            Password = "admin",
            Email    = "admin@gmail.com",
            ProfilePicture = null,
        };

        var check_admin = _db.Accounts.SingleOrDefault(a => a.Username == adminAccount.Username && a.Password == adminAccount.Password && a.Email == adminAccount.Email);
        if (check_admin == null)
        {
            _db.Accounts.Add(adminAccount);
            _db.SaveChanges();
        }
     
        
        string usersession = HttpContext.Session.GetString("Usersession");
        if (usersession != null) {
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
        model.NewPost.User_Picture = account.ProfilePicture;
        model.NewPost.timeCreate = DateTime.Now;

        DateTime timeEnd = model.NewPost.dayend + model.NewPost.timeend; // รวม DateTime

        model.NewPost.timeout = timeEnd;

        _db.Posts.Add(model.NewPost); // เอาเฉพาะ Newpost มาเก็บลง DB
        _db.SaveChanges(); // บันทึกข้อมูล
        return RedirectToAction("Index"); // กลับไปหน้า Index

    }

    [HttpPost]
    public IActionResult Joinroom(int? id, string user)
    {
        var account = _db.Accounts.SingleOrDefault(account => account.Username == user);


        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _db.Posts.Find(id);
        if (obj == null)
        {
            return NotFound(); // ถ้าไม่พบโพสต์
        }

        if (obj.User_list.Contains(account.ID))
        {
            return Json(new { success = false});
        }
        else
        {
            obj.Count_person++;
            obj.User_list.Add(account.ID);
        }

        _db.SaveChanges();

        // คืนค่าตอบกลับ JSON ที่มีข้อมูลใหม่
        return Json(new { success = true, newCount = obj.Count_person });
    }


    


}
