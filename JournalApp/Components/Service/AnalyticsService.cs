using JournalApp.Components.Models;
using SQLite;

namespace JournalApp.Components.Service
{
    public class AnalyticsService
    {
        private readonly DatabaseService _databaseService;
        private readonly IJournalEntryService _journalEntryService;
        private readonly TagService _tagService;

        public AnalyticsService(DatabaseService databaseService, IJournalEntryService journalEntryService, TagService tagService)
        {
            _databaseService = databaseService;
            _journalEntryService = journalEntryService;
            _tagService = tagService;
        }

        //mood distribution
        public async Task<Dictionary<string, int>> GetMoodDistributionAsync(int userId)
        {
            var db = _databaseService.GetConnection();
            var entries = await _journalEntryService.GetAllEntriesAsync(userId);

            var moodCounts = new Dictionary<string, int>();

            foreach (var entry in entries)
            {
                if (entry.PrimaryMoodID.HasValue)
                {
                    var mood = await db.FindAsync<Mood>(entry.PrimaryMoodID.Value);
                    if (mood != null)
                    {
                        if (moodCounts.ContainsKey(mood.MoodName))
                            moodCounts[mood.MoodName]++;
                        else
                            moodCounts[mood.MoodName] = 1;
                    }
                }
            }

            return moodCounts.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        //getting mood category distribution
        public async Task<Dictionary<string, int>> GetMoodCategoryDistributionAsync(int userId)
        {
            var db = _databaseService.GetConnection();
            var entries = await _journalEntryService.GetAllEntriesAsync(userId);

            var categoryCounts = new Dictionary<string, int>
            {
                { "Positive", 0 },
                { "Neutral", 0 },
                { "Negative", 0 }
            };

            foreach (var entry in entries)
            {
                if (entry.PrimaryMoodID.HasValue)
                {
                    var mood = await db.FindAsync<Mood>(entry.PrimaryMoodID.Value);
                    if (mood != null && categoryCounts.ContainsKey(mood.Category))
                    {
                        categoryCounts[mood.Category]++;
                    }
                }
            }

            return categoryCounts;
        }

        //Most frequent moods
        public async Task<List<(string moodName, int count)>> GetMostFrequentMoodsAsync(int userId, int limit = 5)
        {
            var moodDistribution = await GetMoodDistributionAsync(userId);
            return moodDistribution
                .OrderByDescending(x => x.Value)
                .Take(limit)
                .Select(x => (x.Key, x.Value))
                .ToList();
        }

        //word count trends overtime
        public async Task<Dictionary<DateTime, int>> GetWordCountTrendsAsync(int userId, int days = 30)
        {
            var startDate = DateTime.Now.Date.AddDays(-days);
            var entries = await _journalEntryService.GetEntriesByDateRangeAsync(userId, startDate, DateTime.Now);

            return entries
                .GroupBy(e => e.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.WordCount));
        }

        //average word count
        public async Task<double> GetAverageWordCountAsync(int userId)
        {
            var entries = await _journalEntryService.GetAllEntriesAsync(userId);
            if (entries.Count == 0)
                return 0;

            return entries.Average(e => e.WordCount);
        }

        //most used tags
        public async Task<List<(string tagName, int count)>> GetTagBreakdownAsync(int userId, int limit = 10)
        {
            var mostUsedTags = await _tagService.GetMostUsedTagsAsync(userId, limit);
            return mostUsedTags.Select(t => (t.tag.TagName, t.count)).ToList();
        }

        //entries by month
        public async Task<Dictionary<string, int>> GetEntriesByMonthAsync(int userId, int months = 12)
        {
            var startDate = DateTime.Now.AddMonths(-months);
            var entries = await _journalEntryService.GetEntriesByDateRangeAsync(userId, startDate, DateTime.Now);

            return entries
                .GroupBy(e => e.CreatedAt.ToString("MMM yyyy"))
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}