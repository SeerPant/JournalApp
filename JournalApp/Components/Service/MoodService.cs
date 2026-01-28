using JournalApp.Components.Models;
using SQLite;

namespace JournalApp.Components.Service
{
    public class MoodService
    {
        private readonly DatabaseService _databaseService;

        public MoodService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        //getting all moods
        public async Task<List<Mood>> GetAllMoodsAsync()
        {
            var db = _databaseService.GetConnection();
            return await db.Table<Mood>().ToListAsync();
        }

        //getting moods by category
        //public async Task<List<Mood>> GetMoodsByCategoryAsync(string category)
        //{
        //    var db = _databaseService.GetConnection();
        //    return await db.Table<Mood>()
        //        .Where(m => m.Category == category)
        //        .ToListAsync();
        //}

        //getting mood by ID
        public async Task<Mood?> GetMoodByIdAsync(int moodId)
        {
            var db = _databaseService.GetConnection();
            return await db.FindAsync<Mood>(moodId);
        }

    }
}