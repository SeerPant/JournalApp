using JournalApp.Components.Service;
using System;
using System.Threading.Tasks;

namespace JournalApp.Components.ViewModels
{
    public class RegisterViewModel
    {
        private readonly IUserService _userService;

        
        public string Username { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public string ConfirmPin { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsLoading { get; set; } = false;

        
        public RegisterViewModel(IUserService userService)
        {
            _userService = userService;
        }

        
        public async Task<bool> RegisterAsync()
        {
            //validating user name
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Please enter a username";
                return false;
            }

            //validating pin
            if (string.IsNullOrWhiteSpace(Pin))
            {
                ErrorMessage = "Please enter a PIN";
                return false;
            }

            //checking pin length
            if (Pin.Length < 4)
            {
                ErrorMessage = "PIN must be at least 4 characters";
                return false;
            }

            //checking pin matching
            if (Pin != ConfirmPin)
            {
                ErrorMessage = "PINs do not match";
                return false;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                //creating user
                bool success = await _userService.CreateUserAsync(Username, Pin);

                if (success)
                {
                    return true; 
                }
                else
                {
                    ErrorMessage = "User already exists";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Registration failed: {ex.Message}";
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        //Checking if user already exists
        public async Task<bool> CheckUserExistsAsync()
        {
            return await _userService.HasUserAsync();
        }

        //clearing old data
        public void Clear()
        {
            Username = string.Empty; 
            Pin = string.Empty; 
            ConfirmPin = string.Empty; 
            ErrorMessage = string.Empty; 
            IsLoading = false;
        }
    }
}