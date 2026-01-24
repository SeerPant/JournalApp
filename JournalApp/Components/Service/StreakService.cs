using JournalApp.Components.Models;
using SQLite;

namespace JournalApp.Components.Service
{
    public class StreakService
    {
        private readonly DatabaseService _databaseService;
        private readonly IJournalEntryService _journalEntryService;

        public StreakService(DatabaseService databaseService, IJournalEntryService journalEntryService)
        {
            _databaseService = databaseService;
            _journalEntryService = journalEntryService;
        }

        //getting or creating streak for user
        public async Task<Streak> GetOrCreateStreakAsync(int userId)
        {
            var db = _databaseService.GetConnection();
            var streak = await db.Table<Streak>()
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();

            if (streak == null)
            {
                streak = new Streak
                {
                    UserId = userId,
                    CurrentStreak = 0,
                    LongestStreak = 0,
                    TotalEntries = 0,
                    MissedDays = 0,
                    UpdatedAt = DateTime.Now
                };
                await db.InsertAsync(streak);
            }

            return streak;
        }

        //updating streak after creating entry
        public async Task UpdateStreakAsync(int userId)
        {
            var db = _databaseService.GetConnection();
            var streak = await GetOrCreateStreakAsync(userId);
            var today = DateTime.Now.Date;

            //getting total entries
            streak.TotalEntries = await _journalEntryService.GetTotalEntriesCountAsync(userId);

            //checking today's entry
            var hasToday = await _journalEntryService.HasEntryForDateAsync(today, userId);

            if (hasToday)
            {
                //checking last entry 
                if (streak.LastEntryDate.HasValue)
                {
                    var daysSinceLastEntry = (today - streak.LastEntryDate.Value.Date).Days;

                    if (daysSinceLastEntry == 1)
                    {
                        //continue streak
                        streak.CurrentStreak++;
                    }
                    else if (daysSinceLastEntry > 1)
                    {
                        //set streak to 1
                        streak.MissedDays += daysSinceLastEntry - 1;
                        streak.CurrentStreak = 1;
                    }
                    
                }
                else
                {
                    //first entry
                    streak.CurrentStreak = 1;
                }

                streak.LastEntryDate = today;

                //updating longest streak
                if (streak.CurrentStreak > streak.LongestStreak)
                {
                    streak.LongestStreak = streak.CurrentStreak;
                }
            }

            streak.UpdatedAt = DateTime.Now;
            await db.UpdateAsync(streak);
        }

        //calculating missed days
        public async Task<int> CalculateMissedDaysAsync(int userId)
        {
            var entries = await _journalEntryService.GetAllEntriesAsync(userId);
            if (entries.Count == 0)
                return 0;

            var orderedDates = entries.Select(e => e.CreatedAt.Date).Distinct().OrderBy(d => d).ToList();
            
            if (orderedDates.Count < 2)
                return 0;

            int missedDays = 0;
            for (int i = 0; i < orderedDates.Count - 1; i++)
            {
                var daysBetween = (orderedDates[i + 1] - orderedDates[i]).Days - 1;
                if (daysBetween > 0)
                    missedDays += daysBetween;
            }

            return missedDays;
        }
    }
}