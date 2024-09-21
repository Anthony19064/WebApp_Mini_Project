using WebApp_Mini_Project.Models;

namespace WebApp_Mini_Project.Models
{
    public class AccountViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public Account account { get; set; }
    }
}
