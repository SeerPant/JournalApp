using JournalApp.Components.Models;
using SQLite;

namespace JournalApp.Components.Service
{
    public class TagService
    {
        private readonly DatabaseService _databaseService;

        public TagService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        //getting all tags
        public async Task<List<Tag>> GetAllTagsAsync()
        {
            var db = _databaseService.GetConnection();
            return await db.Table<Tag>()
                .OrderBy(t => t.TagName)
                .ToListAsync();
        }

        //getting all tags by ID
        public async Task<Tag?> GetTagByIdAsync(int tagId)
        {
            var db = _databaseService.GetConnection();
            return await db.FindAsync<Tag>(tagId);
        }

        // Get predefined tags
        public async Task<List<Tag>> GetPredefinedTagsAsync()
        {
            var db = _databaseService.GetConnection();
            return await db.Table<Tag>()
                .Where(t => t.IsPreDefined == true)
                .ToListAsync();
        }

        // Create custom tag
        public async Task<int> CreateTagAsync(string tagName)
        {
            // Check if tag already exists
            var db = _databaseService.GetConnection();
            var existing = await db.Table<Tag>()
                .Where(t => t.TagName == tagName)
                .FirstOrDefaultAsync();

            if (existing != null)
                return existing.TagID;

            var tag = new Tag
            {
                TagName = tagName,
                IsPreDefined = false,
                CreatedAt = DateTime.Now
            };

            await db.InsertAsync(tag);
            return tag.TagID;
        }

        //getting tags for entry
        public async Task<List<Tag>> GetTagsForEntryAsync(int entryId)
        {
            var db = _databaseService.GetConnection();
            
            var entryTags = await db.Table<EntryTag>()
                .Where(et => et.EntryID == entryId)
                .ToListAsync();

            var tags = new List<Tag>();
            foreach (var et in entryTags)
            {
                var tag = await db.FindAsync<Tag>(et.TagID);
                if (tag != null)
                    tags.Add(tag);
            }

            return tags;
        }

        //adding tag to entry
        public async Task AddTagToEntryAsync(int entryId, int tagId)
        {
            var db = _databaseService.GetConnection();
            
            // Check if already exists
            var existing = await db.Table<EntryTag>()
                .Where(et => et.EntryID == entryId && et.TagID == tagId)
                .FirstOrDefaultAsync();

            if (existing == null)
            {
                var entryTag = new EntryTag
                {
                    EntryID = entryId,
                    TagID = tagId
                };
                await db.InsertAsync(entryTag);
            }
        }

        //removing tags from entry
        public async Task RemoveTagFromEntryAsync(int entryId, int tagId)
        {
            var db = _databaseService.GetConnection();
            var entryTag = await db.Table<EntryTag>()
                .Where(et => et.EntryID == entryId && et.TagID == tagId)
                .FirstOrDefaultAsync();

            if (entryTag != null)
            {
                await db.DeleteAsync(entryTag);
            }
        }

        //getting most used tags
        public async Task<List<(Tag tag, int count)>> GetMostUsedTagsAsync(int userID, int limit = 10)
        {
            var db = _databaseService.GetConnection();
            
            var entries = await db.Table<JournalEntry>()
                .Where(e => e.UserID == userID)
                .ToListAsync();

            var entryIds = entries.Select(e => e.EntryID).ToList();

            var entryTags = await db.Table<EntryTag>()
                .Where(et => entryIds.Contains(et.EntryID))
                .ToListAsync();

            var tagCounts = entryTags
                .GroupBy(et => et.TagID)
                .Select(g => new { TagId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(limit)
                .ToList();

            var result = new List<(Tag tag, int count)>();
            foreach (var tc in tagCounts)
            {
                var tag = await db.FindAsync<Tag>(tc.TagId);
                if (tag != null)
                    result.Add((tag, tc.Count));
            }

            return result;
        }
    }
}