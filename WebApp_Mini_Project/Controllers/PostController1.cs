using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp_Mini_Project.Models;

namespace WebApp_Mini_Project.Controllers
{
    public class PostController : Controller
    {
        // ตัวอย่างข้อมูลจำลอง
        private static List<Post> posts = new List<Post>
        {
            new Post("AJMCJ19", 5, "หาสาวเสียงน่ารักๆ", "Valorant"),
            new Post("1748200", 3, "หาควายให้แบกขอแรงค์ iron", "Rov"),
            new Post("xxxxxxx", 2, "Gold - Plat ชิลๆ ไม่ซี", "Valorant"),
        };

        // GET: Post
        public ActionResult Index()
        {
            return View(posts);
        }

        // GET: Post/Details/5
        public ActionResult Details(int id)
        {
            var post = posts[id];
            return View(post);
        }
    }
}
