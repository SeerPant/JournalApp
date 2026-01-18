
namespace JournalApp.Components.Models
{
    public class User
    {
        public int userID {get; set;}
        public string userName{get; set;} = string.Empty; 
        public string pinHash {get; set;} = string.Empty;
        public DateTime CreatedDate{get; set;} 
        public DateTime LastLoginDate {get; set;}
    }
}
