namespace WebApp_Mini_Project.Models
{
    public class Post
    {
        public string id_room { get; set; }
        public int person_count { get; set; }
        public string  details { get; set; }
        public string game_name { get; set; }

        public Post(string Id_room , int Person_count , string Details , string Game_name)
        {
            id_room = Id_room;
            person_count = Person_count;
            details = Details;
            game_name = Game_name;
        }

        public void Displayinfo()
        {
            Console.WriteLine($"Id room   : {this.id_room}");
            Console.WriteLine($"Count     : {this.person_count}");
            Console.WriteLine($"Details   : {this.details}");
            Console.WriteLine($"Game name : {this.game_name}");
        }


        
    }
}
