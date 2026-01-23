using JournalApp.Components.Models;

namespace JournalApp.Components.Service
{
    public interface IJournalEntryService
    {
        //CRUD operations
        Task<JournalEntry?> GetEntryByDateAsync(DateTime date, int userId);
        Task<JournalEntry?> GetEntryByIdAsync(int entryId);
        Task<List<JournalEntry>> GetAllEntriesAsync(int userId);
        Task<List<JournalEntry>> GetEntriesByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<bool> CreateEntryAsync(JournalEntry entry);
        Task<bool> UpdateEntryAsync(JournalEntry entry);
        Task<bool> DeleteEntryAsync(int entryId);
        
        //Search operations
        Task<List<JournalEntry>> SearchEntriesAsync(int userId, string searchTerm);
        Task<List<JournalEntry>> FilterByMoodAsync(int userId, int moodId);
        Task<List<JournalEntry>> FilterByCategoryAsync(int userId, int categoryId);
        
        //pagination
        Task<List<JournalEntry>> GetPagedEntriesAsync(int userId, int pageNumber, int pageSize);
        Task<int> GetTotalEntriesCountAsync(int userId);
        
        //validating entry
        Task<bool> HasEntryForDateAsync(DateTime date, int userId);
    }
}