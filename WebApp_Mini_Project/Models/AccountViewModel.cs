using WebApp_Mini_Project.Models;

namespace WebApp_Mini_Project.Models
{
    public class AccountViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Account> Account_list { get; set; }
        public Account Account { get; set; }
    }
}
