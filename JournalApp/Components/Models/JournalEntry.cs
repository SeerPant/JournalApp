using System;
using System.Collections.Generic;
using SQLite;
namespace JournalApp.Components.Models
{
    public class JournalEntry
    {
        [PrimaryKey, AutoIncrement] 
        public int EntryID {get; set;}

        [NotNull]
        public int UserID{get; set;}

        [MaxLength(200)] 
        public string Title {get; set;} = string.Empty;

        [NotNull] 
        public string Content {get; set;} = string.Empty;

        public int? PrimaryMoodID {get; set;}
        public int? SecondaryMoodFirstID {get; set;}
        public int? SecondaryMoodSecondID{get; set;} 

        public int? CategoryID {get;set;} 

        public int WordCount{get; set;} 

        [NotNull]

        public DateTime CreatedAt {get; set;} 

        [NotNull] 
        public DateTime UpdateAt{get; set;}

    }
}