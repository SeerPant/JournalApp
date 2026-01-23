using SQLite;

namespace JournalApp.Components.Models
{
    public class Streak
    {
        [PrimaryKey, AutoIncrement]
        public int StreakId { get; set; }

        [NotNull]
        public int UserId { get; set; }

        public int CurrentStreak { get; set; } = 0;

        public int LongestStreak { get; set; } = 0;

        public DateTime? LastEntryDate { get; set; }

        public int TotalEntries { get; set; } = 0;

        public int MissedDays { get; set; } = 0;

        [NotNull]
        public DateTime UpdatedAt { get; set; }
    }
}