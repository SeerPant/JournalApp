using SQLite; 

namespace JournalApp.Components.Models
{
    public class Mood
    {
        [PrimaryKey, AutoIncrement]
        public int MoodID {get; set;}

        [MaxLength(50), NotNull]
        public string MoodName {get; set;} = string.Empty;

        [MaxLength(20), NotNull] 
        public string Category {get; set;} = string.Empty; 

        public bool IsPreDefined {get; set;} = true;



    }
}