using Microsoft.AspNetCore.Mvc;
using WebApp_Mini_Project.Data;
using WebApp_Mini_Project.Models;
using WebApp_Mini_Project.ViewModels;

public class PostController : Controller
{
    public readonly ApplicationDBContext _db;

    public PostController(ApplicationDBContext db)
    {
        _db = db;
    }

    public ActionResult Index()
    {
        var posts = _db.Posts; // ข้อมูลใน DB เก็บลง ตัวแปร posts
        var newPost = new Post(); // ข้อมูลที่รับมาจากฟอรม์ เก็บลง ตัวแปร newPost

        var viewModel = new PostViewModel // เอาสองตัวบนมาเก็บลง object ViewModel
        {
            Posts = posts,
            NewPost = newPost
        };

        return View(viewModel); // ส่ง ViewModel ไปยัง View index
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(PostViewModel model) // สร้างตัวแปร PostViewModel ที่รับค่าจากสองตัวเมื่อกี้ มาเก็บลงตัวแปร model
    {
  
            _db.Posts.Add(model.NewPost); // เอาเฉพาะ Newpost มาเก็บลง DB
            _db.SaveChanges(); // บันทึกข้อมูล
            return RedirectToAction("Index"); // กลับไปหน้า Index

    }

    [HttpPost]
    public IActionResult Joinroom(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var obj = _db.Posts.Find(id);

        if (obj == null)
        {
            return NotFound(); // ถ้าไม่พบโพสต์
        }

        obj.Count_person++;
        _db.SaveChanges();

        // คืนค่าตอบกลับ JSON ที่มีข้อมูลใหม่
        return Json(new { success = true, newCount = obj.Count_person });
    }



}
