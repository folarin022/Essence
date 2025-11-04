using System.Text.RegularExpressions;

namespace EssenceShop.Services
{
    public static class ValidationService
    {
        public static bool IsValidEmail(string email) =>
            Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public static bool IsValidPassword(string password) =>
    Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""':{}|<>]).{8,}$");



    }
}
