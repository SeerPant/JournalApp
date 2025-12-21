using System;
using System.Collections.Generic;

namespace JournalApp.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Mood tracking
        public string PrimaryMood { get; set; } = string.Empty;
        public string? SecondaryMood1 { get; set; }
        public string? SecondaryMood2 { get; set; }

        // Tags
        public List<string> Tags { get; set; } = new();
    }

    public static class MoodCategories
    {
        public static readonly Dictionary<string, List<string>> Moods = new()
        {
            { "Positive", new List<string> { "Happy", "Excited", "Relaxed", "Grateful", "Confident" } },
            { "Neutral", new List<string> { "Calm", "Thoughtful", "Curious", "Nostalgic", "Bored" } },
            { "Negative", new List<string> { "Sad", "Angry", "Stressed", "Lonely", "Anxious" } }
        };

        public static List<string> GetAllMoods()
        {
            var allMoods = new List<string>();
            foreach (var category in Moods.Values)
            {
                allMoods.AddRange(category);
            }
            return allMoods;
        }
    }

    public static class PrebuiltTags
    {
        public static readonly List<string> Tags = new()
        {
            "Work", "Career", "Studies", "Family", "Friends", "Relationships",
            "Health", "Fitness", "Personal Growth", "Self-care", "Hobbies",
            "Travel", "Nature", "Finance", "Spirituality", "Birthday",
            "Holiday", "Vacation", "Celebration", "Exercise", "Reading",
            "Writing", "Cooking", "Meditation", "Yoga", "Music", "Shopping",
            "Parenting", "Projects", "Planning", "Reflection"
        };
    }
}