using Microsoft.EntityFrameworkCore;
using WebApp_Mini_Project.Models;

namespace WebApp_Mini_Project.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Chat> Chats { get; set; }



    }
}