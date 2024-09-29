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
        [Required]
        public List<int>? User_list { get; set; } = new List<int>();
        [Required]
        public DateTime timeCreate { get; set; }
        [Required]
        public DateTime timeout { get; set; }
        [NotMapped]
        public DateTime dayend { get; set; }
        [NotMapped]
        public TimeSpan timeend { get; set; }
        [Required]
        public bool status { get; set; } = true;









    }
}
