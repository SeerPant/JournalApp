using SQLite; 
using JournalApp.Components.Models; 

namespace JournalApp.Components.Service
{
    public class DatabaseService
    {   
        //variable to hold connection
        private readonly SQLiteAsyncConnection database;
        private readonly Task _initializationTask;

        public DatabaseService()
        {
            //variable storing the database's path
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "JournalApp",
                "JournalApp.db"
            );

            //create directory if it does not exist
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

            //initialize sqlite connection 
            database = new SQLiteAsyncConnection(dbPath); 

            //initialize database tables asynchronously without blocking
            _initializationTask = InitializeDatabaseAsync();
        }

        //method to ensure database is initialized before use
        public async Task EnsureInitializedAsync()
        {
            await _initializationTask;
        }

        //method to initialize database 
        private async Task InitializeDatabaseAsync()
        {
            try
            {
                //create user table 
                await database.CreateTableAsync<User>();
                await database.CreateTableAsync<JournalEntry>(); 
                await database.CreateTableAsync<Mood>(); 
                await database.CreateTableAsync<Tag>(); 
                await database.CreateTableAsync<EntryTag>(); 
                await database.CreateTableAsync<Category>(); 
                await database.CreateTableAsync<Streak>();

                //seeding predefined data
                await SeedPredefinedDataAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
                throw;
            }
        }

        //seeding data in database 
        private async Task SeedPredefinedDataAsync()
        {
            //seed user if user doesnt exist
            var userCount = await database.Table<User>().CountAsync();
            if (userCount == 0)
            {
                var defaultUser = new User
                {
                    userName = "DefaultUser",
                    pinHash = HashPin("1234"), // Default PIN
                    CreatedDate = DateTime.Now,
                    LastLoginDate = DateTime.Now
                };

                await database.InsertAsync(defaultUser);
                System.Diagnostics.Debug.WriteLine("Default user created with PIN: 1234");
            }

            // Seed predefined moods if not already present
            var moodCount = await database.Table<Mood>().CountAsync();
            if (moodCount == 0)
            {
                var predefinedMoods = new List<Mood>
                {
                    //positive moods
                    new Mood { MoodName = "Happy", Category = "Positive", IsPreDefined = true },
                    new Mood { MoodName = "Excited", Category = "Positive", IsPreDefined = true },
                    new Mood { MoodName = "Relaxed", Category = "Positive", IsPreDefined = true },
                    new Mood { MoodName = "Grateful", Category = "Positive", IsPreDefined = true },
                    new Mood { MoodName = "Confident", Category = "Positive", IsPreDefined = true },
                    
                    //neutral moods
                    new Mood { MoodName = "Calm", Category = "Neutral", IsPreDefined = true },
                    new Mood { MoodName = "Thoughtful", Category = "Neutral", IsPreDefined = true },
                    new Mood { MoodName = "Curious", Category = "Neutral", IsPreDefined = true },
                    new Mood { MoodName = "Nostalgic", Category = "Neutral", IsPreDefined = true },
                    new Mood { MoodName = "Bored", Category = "Neutral", IsPreDefined = true },
                    
                    //negative moods
                    new Mood { MoodName = "Sad", Category = "Negative", IsPreDefined = true },
                    new Mood { MoodName = "Angry", Category = "Negative", IsPreDefined = true },
                    new Mood { MoodName = "Stressed", Category = "Negative", IsPreDefined = true },
                    new Mood { MoodName = "Lonely", Category = "Negative", IsPreDefined = true },
                    new Mood { MoodName = "Anxious", Category = "Negative", IsPreDefined = true }
                };

                await database.InsertAllAsync(predefinedMoods);
            }

            //seeding predefined tags if not present
            var tagCount = await database.Table<Tag>().CountAsync();
            if (tagCount == 0)
            {
                var predefinedTags = new List<Tag>
                {
                    new Tag { TagName = "Work", IsPreDefined = true, CreatedAt = DateTime.Now },
                    new Tag { TagName = "Health", IsPreDefined = true, CreatedAt = DateTime.Now },
                    new Tag { TagName = "Travel", IsPreDefined = true, CreatedAt = DateTime.Now },
                    new Tag { TagName = "Fitness", IsPreDefined = true, CreatedAt = DateTime.Now },
                    new Tag { TagName = "Family", IsPreDefined = true, CreatedAt = DateTime.Now },
                    new Tag { TagName = "Friends", IsPreDefined = true, CreatedAt = DateTime.Now },
                    new Tag { TagName = "Personal", IsPreDefined = true, CreatedAt = DateTime.Now },
                    new Tag { TagName = "Goals", IsPreDefined = true, CreatedAt = DateTime.Now }
                };

                await database.InsertAllAsync(predefinedTags);
            }

            //seeding predefined categories if not present
            var categoryCount = await database.Table<Category>().CountAsync();
            if (categoryCount == 0)
            {
                var predefinedCategories = new List<Category>
                {
                    new Category { CategoryName = "Daily Log", Description = "Regular daily entries", IsPreDefined = true, CreatedAt = DateTime.Now },
                    new Category { CategoryName = "Reflection", Description = "Deep thoughts and reflections", IsPreDefined = true, CreatedAt = DateTime.Now },
                    new Category { CategoryName = "Gratitude", Description = "Things you're grateful for", IsPreDefined = true, CreatedAt = DateTime.Now },
                    new Category { CategoryName = "Dreams", Description = "Dream journal entries", IsPreDefined = true, CreatedAt = DateTime.Now }
                };

                await database.InsertAllAsync(predefinedCategories);
            }
        }
        //hashing pin of seeded user
        private string HashPin(string pin)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(pin);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        public SQLiteAsyncConnection GetConnection()
        {
            return database;
        }
    }
}