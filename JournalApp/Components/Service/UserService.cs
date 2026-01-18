using JournalApp.Components.Service;
using JournalApp.Components.Models;
using Microsoft.Data.Sqlite;
using System; 
using System.Security.Cryptography; 
using System.Text; 
using System.Threading.Tasks;

namespace JournalApp.Components.Service
{
    public class UserService : IUserService
    {
        private readonly DatabaseService _databaseService; 

        public UserService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<User?> GetUserAsync()
        {
            using var connection = _databaseService.GetConnection(); 
            await connection.OpenAsync(); 

            var command = connection.CreateCommand(); 
            //command to get user
            command.CommandText = "SELECT userID, userName, pinHash, CreatedDate, LastLoginDate FROM Users LIMIT 1";

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    userID = reader.GetInt32(0),
                    userName = reader.GetString(1),
                    pinHash = reader.GetString(2),
                    CreatedDate = DateTime.Parse(reader.GetString(3)),
                    LastLoginDate = DateTime.Parse(reader.GetString(4))
                };
            }
            return null;
        }   

        public async Task<bool> HasUserAsync()
        {
            using var connection = _databaseService.GetConnection();
            await connection.OpenAsync(); 


            var command = connection.CreateCommand(); 
            command.CommandText = "SELECT COUNT(*) FROM Users";

            //ExecuteScalar because only 1 object is being returned
            //and type casting to long integer 'long'
            var count = (long)(await command.ExecuteScalarAsync()?? 0);
            return count>0;
        }
        
        private string HashPin(string pin)
        {   
            //creating sha256 encryption
            using var sha256 = SHA256.Create();
            //converting to bytes
            var bytes = Encoding.UTF8.GetBytes(pin);
            //hashing the PIN
            var hash = sha256.ComputeHash(bytes); 
            //converting to string
            return Convert.ToBase64String(hash);
        }
        public async Task<bool> CreateUserAsync(string username, string pin)
        {   
            //return false, if user exists
            if (await HasUserAsync()) return false;
            //return false, if username or pin is invalid
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(pin)) return false;

            using var connection = _databaseService.GetConnection(); 
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Users(userName, pinHash, CreatedDate, LastLoginDate) VALUES (@username, @pinHash, @createdDate, @lastLoginDate)";
                command.Parameters.AddWithValue("@username", username.Trim());
                //hashing the pin
                command.Parameters.AddWithValue("@pinHash", HashPin(pin));
                command.Parameters.AddWithValue("@createdDate", DateTime.Now.ToString("o")); 
                command.Parameters.AddWithValue("@lastLoginDate",DateTime.Now.ToString("o"));

                await command.ExecuteNonQueryAsync(); 
                return true;
        }

        public async Task<bool> ValidatePinAsync(string pin)
        {   
            //getting user
            var user = await GetUserAsync();
            //check for user
            if (user == null) return false; 
            //comparing pin
            return user.pinHash == HashPin(pin);
        }

        public async Task<bool> UpdatePinAsync(string oldPin, string newPin)
        {   
            //checking if old pin is correct
            if(!await ValidatePinAsync(oldPin)) return false;
            //validating new pin
            if(string.IsNullOrWhiteSpace(newPin)) return false; 

            using var connection = _databaseService.GetConnection(); 
            await connection.OpenAsync();

            //updating the pin in database
            var command = connection.CreateCommand(); 
            command.CommandText = "UPDATE Users SET pinHash = @pinHash WHERE userID = (SELECT userID FROM Users LIMIT 1)";
            command.Parameters.AddWithValue("@pinHash", HashPin(newPin)); 

            //executing
            await command.ExecuteNonQueryAsync(); 
            return true;
        }

        public async Task UpdateLastLoginAsync()
        {
            using var connection = _databaseService.GetConnection();
            await connection.OpenAsync(); 

            var command = connection.CreateCommand(); 

            //command to set last login date
            command.CommandText = "UPDATE Users SET LastLoginDate = @lastLoginDate WHERE userID = (SELECT userID FROM Users LIMIT 1)";
            command.Parameters.AddWithValue("@lastLoginDate", DateTime.Now.ToString("o"));

            await command.ExecuteNonQueryAsync();
        }
        public async Task DeleteUserAsync()
        {
            using var connection = _databaseService.GetConnection();
            await connection.OpenAsync();

            //command to delete user
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Users";

            await command.ExecuteNonQueryAsync();
        }
    }   
}
