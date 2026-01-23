using JournalApp.Components.Models;
using SQLite;

namespace JournalApp.Components.Service
{
    public class CategoryService
    {
        private readonly DatabaseService _databaseService;

        public CategoryService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        //getting all categories
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var db = _databaseService.GetConnection();
            return await db.Table<Category>()
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }
    }
}