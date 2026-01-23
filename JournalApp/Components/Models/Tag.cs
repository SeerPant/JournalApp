using SQLite; 

namespace JournalApp.Components.Models
{
    public class Tag
    {
        [PrimaryKey, AutoIncrement]
        public int TagID{get; set;}

        [MaxLength(50), NotNull, Unique]
        public string TagName{get; set;} = string.Empty; 

        public bool IsPreDefined{get; set;} = false; 

        public DateTime CreatedAt {get; set;}


    }
}