using JournalApp.Components.Service;
using JournalApp.Components.Models;
using SQLite;
using System; 
using System.Security.Cryptography; 
using System.Text; 


namespace JournalApp.Components.Service
{
    public class UserService: IUserService
    {
        private readonly DatabaseService _databaseService;

        //getting database connection
        public UserService (DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        
        //getting list of users
        public async Task<User?> GetUserAsync()
        {
            var db = _databaseService.GetConnection();
            var users = await db.Table<User>().ToListAsync();
            return users.FirstOrDefault();
        }

        //counting users in database
        public async Task<bool> HasUserAsync()
        {
            var db = _databaseService.GetConnection();
            var count = await db.Table<User>().CountAsync(); 
            return count > 0;
        }

        //method to hash the pin 
        private string HashPin(string pin)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(pin);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        //creating user
        //public async Task<bool> CreateUserAsync(string username, string pin)
        //{
        //    if (await HasUserAsync()) return false; 

        //    var user = new User
        //    {
        //        userName = username, 
        //        pinHash = HashPin(pin),
        //        CreatedDate = DateTime.Now, LastLoginDate = DateTime.Now
        //    };

        //    var db = _databaseService.GetConnection(); 
        //    await db.InsertAsync(user); 
        //    return true;
        //}

        //method to validate pin 
        public async Task<bool> ValidatePinAsync(string pin)
        {
            
            var user = await GetUserAsync(); 
            if (user == null) return false;
            return user.pinHash == HashPin(pin);

        }

        //method to update pin 
        public async Task<bool> UpdatePinAsync(string oldPin, string newPin)
        {
            if (!await ValidatePinAsync(oldPin)) return false; 

            var user = await GetUserAsync(); 
            if (user == null) return false; 

            user.pinHash = HashPin(newPin);

            var db = _databaseService.GetConnection(); 
            await db.UpdateAsync(user); 
            return true;
        }

        //method to update last login time 
        public async Task UpdateLastLoginAsync()
        {
            var user = await GetUserAsync(); 
            if (user == null) return; 

            user.LastLoginDate = DateTime.Now; 

            var db = _databaseService.GetConnection(); 
            await db.UpdateAsync(user); 
        }

        //method to delete user  
        //public async Task DeleteUserAsync()
        //{
        //    var user = await GetUserAsync(); 
        //    if (user == null) return; 

        //    var db = _databaseService.GetConnection(); 
        //    await db.DeleteAsync(user);
        //}
    }
}
