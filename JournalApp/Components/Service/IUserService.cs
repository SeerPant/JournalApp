using JournalApp.Components.Models;
using System.Threading.Tasks;

namespace JournalApp.Components.Service
{
    public interface IUserService
    {
        Task<User?> GetUserAsync();
        Task<bool> HasUserAsync();
        //Task<bool> CreateUserAsync(string username, string pin); 
        Task<bool> ValidatePinAsync(string pin);
        //Task<bool> UpdatePinAsync(string oldPin, string newPin);
        Task UpdateLastLoginAsync();
        //Task DeleteUserAsync();
    }
}