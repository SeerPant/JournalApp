using System;
using System.Collections.Generic;
using System.Linq;
using JournalApp.Models;

namespace JournalApp.Services
{
    public class JournalService
    {
        private readonly List<JournalEntry> _entries = new();
        private int _nextId = 1;

        public JournalService()
        {
            // Add some sample data
            SeedData();
        }

        private void SeedData()
        {
            var sampleEntries = new List<JournalEntry>
            {
                new JournalEntry
                {
                    Id = _nextId++,
                    Date = DateTime.Today.AddDays(-2),
                    Title = "A Great Day",
                    Content = "Today was amazing! I finished my project and celebrated with friends. The weather was perfect, and everything just fell into place.",
                    PrimaryMood = "Happy",
                    SecondaryMood1 = "Excited",
                    Tags = new List<string> { "Work", "Friends", "Celebration" },
                    CreatedAt = DateTime.Today.AddDays(-2),
                    UpdatedAt = DateTime.Today.AddDays(-2)
                },
                new JournalEntry
                {
                    Id = _nextId++,
                    Date = DateTime.Today.AddDays(-1),
                    Title = "Reflective Morning",
                    Content = "Spent time thinking about my goals and where I want to be next year. It's important to take these moments to pause and reflect.",
                    PrimaryMood = "Thoughtful",
                    SecondaryMood1 = "Calm",
                    Tags = new List<string> { "Reflection", "Planning", "Personal Growth" },
                    CreatedAt = DateTime.Today.AddDays(-1),
                    UpdatedAt = DateTime.Today.AddDays(-1)
                },
                new JournalEntry
                {
                    Id = _nextId++,
                    Date = DateTime.Today.AddDays(-5),
                    Title = "Morning Workout",
                    Content = "Started the day with an intense workout session. Feeling energized and ready to tackle the day ahead!",
                    PrimaryMood = "Confident",
                    SecondaryMood1 = "Excited",
                    Tags = new List<string> { "Fitness", "Exercise", "Health" },
                    CreatedAt = DateTime.Today.AddDays(-5),
                    UpdatedAt = DateTime.Today.AddDays(-5)
                },
                new JournalEntry
                {
                    Id = _nextId++,
                    Date = DateTime.Today.AddDays(-7),
                    Title = "Peaceful Sunday",
                    Content = "Enjoyed a quiet day at home. Read a good book, cooked a nice meal, and just relaxed. Sometimes these simple days are the best.",
                    PrimaryMood = "Relaxed",
                    SecondaryMood1 = "Grateful",
                    Tags = new List<string> { "Self-care", "Reading", "Cooking" },
                    CreatedAt = DateTime.Today.AddDays(-7),
                    UpdatedAt = DateTime.Today.AddDays(-7)
                }
            };

            _entries.AddRange(sampleEntries);
        }

        public List<JournalEntry> GetAllEntries()
        {
            return _entries.OrderByDescending(e => e.Date).ToList();
        }

        public JournalEntry? GetEntryById(int id)
        {
            return _entries.FirstOrDefault(e => e.Id == id);
        }

        public JournalEntry? GetEntryByDate(DateTime date)
        {
            return _entries.FirstOrDefault(e => e.Date.Date == date.Date);
        }

        public JournalEntry CreateEntry(JournalEntry entry)
        {
            // Check if entry for this date already exists
            if (_entries.Any(e => e.Date.Date == entry.Date.Date))
            {
                throw new InvalidOperationException("An entry for this date already exists.");
            }

            entry.Id = _nextId++;
            entry.CreatedAt = DateTime.Now;
            entry.UpdatedAt = DateTime.Now;
            _entries.Add(entry);
            return entry;
        }

        public void UpdateEntry(JournalEntry entry)
        {
            var existing = _entries.FirstOrDefault(e => e.Id == entry.Id);
            if (existing != null)
            {
                existing.Title = entry.Title;
                existing.Content = entry.Content;
                existing.PrimaryMood = entry.PrimaryMood;
                existing.SecondaryMood1 = entry.SecondaryMood1;
                existing.SecondaryMood2 = entry.SecondaryMood2;
                existing.Tags = entry.Tags;
                existing.UpdatedAt = DateTime.Now;
            }
        }

        public void DeleteEntry(int id)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                _entries.Remove(entry);
            }
        }

        public (List<JournalEntry> Items, int TotalCount) GetPaginatedEntries(int page, int pageSize)
        {
            var totalCount = _entries.Count;
            var items = _entries
                .OrderByDescending(e => e.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            return (items, totalCount);
        }
    }
}