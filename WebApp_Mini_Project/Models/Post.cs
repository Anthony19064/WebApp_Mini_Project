using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp_Mini_Project.Models
{
    public class Post
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Id_room { get; set; }
        [Required]
        public string Name_room {get; set;}
        [Required]
        public int Count_person { get; set; } = 0;
        [Required]
        public int Max_person { get; set; }
        [Required]
        public string  Details { get; set; }
        [Required]
        public string Game { get; set; }
        [Required]
        public int User_id { get; set; }
        public byte[]? User_Picture { get; set; }
        [Required]
        public List<int>? User_list { get; set; } = new List<int>();









    }
}
