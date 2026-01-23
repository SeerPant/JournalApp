using SQLite;

namespace JournalApp.Components.Models
{
    public class User
    {   
        //constraints
        [PrimaryKey, AutoIncrement]
        public int userID {get; set;}

        [MaxLength(50)]
        public string userName{get; set;} = string.Empty; 
        
        [NotNull]
        public string pinHash {get; set;} = string.Empty;
        
        [NotNull]
        public DateTime CreatedDate{get; set;} 
        [NotNull]
        public DateTime LastLoginDate {get; set;}
    }
}
