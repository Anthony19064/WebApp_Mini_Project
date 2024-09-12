using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using WebApp_Mini_Project.Models;

namespace WebApp_Mini_Project.Controllers
{
    public class PostController : Controller
    {
        // ตัวอย่างข้อมูลจำลอง
        private static List<Post> posts = new List<Post>
        {
            new Post("AJMCJ19", 5, "หาสาวเสียงน่ารักๆ", "Valorant"),
            new Post("OQG710", 3, "หาควายให้แบกขอแรงค์ iron", "Valorant"),
            new Post("ZLD174 ", 2, "Gold - Plat ชิลๆ ไม่ซี", "Valorant"),
            new Post("MPY439", 5, "ขาด1ครับ S-G", "Valorant"),
            new Post("SIG009", 3, "ขาด 2 งับ", "Valorant"),
            new Post("VID862", 2, "D-As -25% ขาด 1 จอยตี้ได้เลยครับ", "Valorant"),

        };

        // GET: Post
        public ActionResult Index()
        {
            return View(posts);
        }

        // GET: Post/GetPosts
        public ContentResult GetPosts()
        {
            var options = new JsonSerializerOptions { WriteIndented = true }; // Pretty-print JSON
            string jsonString = JsonSerializer.Serialize(posts, options);
            return Content(jsonString, "application/json"); // ส่ง JSON พร้อม MIME type
        }

        // GET: Post/Details/5
        public ActionResult Details(int id)
        {
            var post = posts[id];
            return View(post);
        }
    }
}
