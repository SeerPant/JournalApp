using SQLite; 

namespace JournalApp.Components.Models
{
    public class EntryTag
    {
        [PrimaryKey, AutoIncrement]
        public int EntryTagID {get; set;}

        [NotNull] 
        public int EntryID {get;set;} 

        [NotNull]
        public int TagID {get; set;}
    }
}
