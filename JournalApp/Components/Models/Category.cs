using SQLite; 

namespace JournalApp.Components.Models
{
    public class Category
    {   
        [PrimaryKey, AutoIncrement]
        public int CategoryID {get; set;}

        [MaxLength(50), NotNull, Unique]
        public string CategoryName {get; set;} = string.Empty; 

        
        public string? Description {get; set;} 

        public bool IsPreDefined {get; set;} = false; 

        public DateTime CreatedAt {get; set;}

    }

}