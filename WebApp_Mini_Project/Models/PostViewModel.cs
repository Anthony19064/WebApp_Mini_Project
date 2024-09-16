using WebApp_Mini_Project.Models;

namespace WebApp_Mini_Project.ViewModels
{
    public class PostViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public Post NewPost { get; set; }
    }
}
