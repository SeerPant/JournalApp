using JournalApp.Components.Models;
using SQLite;

namespace JournalApp.Components.Service
{
    public class JournalEntryService : IJournalEntryService
    {
        private readonly DatabaseService _databaseService;

        public JournalEntryService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        //getting entry for specific date 
        public async Task<JournalEntry?> GetEntryByDateAsync(DateTime date, int userID)
        {
            var db = _databaseService.GetConnection();
            var dateOnly = date.Date;

            // Load all user entries first, then filter by date in memory
            var allEntries = await db.Table<JournalEntry>()
                .Where(e => e.UserID == userID)
                .ToListAsync();

            return allEntries.FirstOrDefault(e => e.CreatedAt.Date == dateOnly);
        }

        //getting entry by ID
        public async Task<JournalEntry?> GetEntryByIdAsync(int entryID)
        {
            var db = _databaseService.GetConnection();
            return await db.FindAsync<JournalEntry>(entryID);
        }

        //getting all etnries 
        public async Task<List<JournalEntry>> GetAllEntriesAsync(int userId)
        {
            var db = _databaseService.GetConnection();
            return await db.Table<JournalEntry>()
                .Where(e => e.UserID == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        //getting etnries by date range
        public async Task<List<JournalEntry>> GetEntriesByDateRangeAsync(int userID, DateTime startDate, DateTime endDate)
        {
            var db = _databaseService.GetConnection();
            return await db.Table<JournalEntry>()
                .Where(e => e.UserID == userID && e.CreatedAt >= startDate && e.CreatedAt <= endDate)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        //checking entry for current date 
        public async Task<bool> HasEntryForDateAsync(DateTime date, int userID)
        {
            var db = _databaseService.GetConnection();
            var dateOnly = date.Date;

            // Load all user entries first, then check date in memory
            var allEntries = await db.Table<JournalEntry>()
                .Where(e => e.UserID == userID)
                .ToListAsync();

            return allEntries.Any(e => e.CreatedAt.Date == dateOnly);
        }

        //counting words in content 
        private int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;

            return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        //creating new entry 
        public async Task<bool> CreateEntryAsync(JournalEntry entry)
        {
            //checking if entry is present, returns false if entry is present
            if (await HasEntryForDateAsync(entry.CreatedAt, entry.UserID)) return false;

            entry.CreatedAt = DateTime.Now;
            entry.UpdateAt = DateTime.Now;
            entry.WordCount = CountWords(entry.Content);

            var db = _databaseService.GetConnection();
            await db.InsertAsync(entry);
            return true;
        }

        //updating existing entry 
        public async Task<bool> UpdateEntryAsync(JournalEntry entry)
        {
            entry.UpdateAt = DateTime.Now;
            entry.WordCount = CountWords(entry.Content);

            var db = _databaseService.GetConnection();
            await db.UpdateAsync(entry);
            return true;
        }

        //deleting entry 
        public async Task<bool> DeleteEntryAsync(int entryID)
        {
            var db = _databaseService.GetConnection();
            var entry = await db.FindAsync<JournalEntry>(entryID);
            if (entry == null) return false;

            await db.DeleteAsync(entry);
            return true;
        }

        //searching entries by title/content 
        public async Task<List<JournalEntry>> SearchEntriesAsync(int userID, string keyword)
        {
            var db = _databaseService.GetConnection();
            //getting entries of the matching user
            var allEntries = await db.Table<JournalEntry>()
                .Where(e => e.UserID == userID)
                .ToListAsync();

            //filtering content and viewing them as newest first
            return allEntries
                .Where(e => e.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                           e.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        //filtering by mood  
        public async Task<List<JournalEntry>> FilterByMoodAsync(int userID, int moodID)
        {
            var db = _databaseService.GetConnection();

            //getting entries of user
            var allEntries = await db.Table<JournalEntry>()
                .Where(e => e.UserID == userID)
                .ToListAsync();

            return allEntries
                .Where(e => e.PrimaryMoodID == moodID ||
                            e.SecondaryMoodFirstID == moodID ||
                            e.SecondaryMoodSecondID == moodID)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        //filtering by category 
        public async Task<List<JournalEntry>> FilterByCategoryAsync(int userID, int categoryID)
        {
            var db = _databaseService.GetConnection();
            return await db.Table<JournalEntry>()
                .Where(e => e.UserID == userID && e.CategoryID == categoryID)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        //getting paginated entries 
        public async Task<List<JournalEntry>> GetPagedEntriesAsync(int userId, int pageNumber, int pageSize)
        {
            var db = _databaseService.GetConnection();
            return await db.Table<JournalEntry>()
                .Where(e => e.UserID == userId)
                .OrderByDescending(e => e.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        //getting count of total entries
        public async Task<int> GetTotalEntriesCountAsync(int userId)
        {
            var db = _databaseService.GetConnection();
            return await db.Table<JournalEntry>()
                .Where(e => e.UserID == userId)
                .CountAsync();
        }
    }
}