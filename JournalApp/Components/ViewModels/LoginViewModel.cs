using JournalApp.Components.Service;
using System;
using System.Threading.Tasks;

namespace JournalApp.Components.ViewModels
{
    public class LoginViewModel
    {
        private readonly IUserService _userService;

        //public fields
        public string Pin { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsLoading { get; set; } = false;

        // Constructor
        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
        }

        //login logic
        public async Task<bool> LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Pin))
            {
                ErrorMessage = "Please enter PIN";
                return false;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                //Pin Validation
                bool isValid = await _userService.ValidatePinAsync(Pin);

                if (isValid)
                {
                    //updating last login 
                    await _userService.UpdateLastLoginAsync();
                    return true;
                }
                else
                {
                    ErrorMessage = "Incorrect PIN. Please try again.";
                    Pin = string.Empty; // Clearing the PIN field
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        //checking if user exists
        public async Task<bool> CheckUserExistsAsync()
        {
            return await _userService.HasUserAsync();
        }

        //method to clear all data 
        public void Clear()
        {
            Pin = string.Empty; 
            ErrorMessage = string.Empty; 
            IsLoading = false;
        }
    }
}