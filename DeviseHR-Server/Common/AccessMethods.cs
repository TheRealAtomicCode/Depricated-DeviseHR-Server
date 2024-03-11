

using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.UserRepository;

namespace DeviseHR_Server.Common
{
    public static class AccessMethods
    {
        public static void VerifyUserAccess(User user)
        {
            if (user.Company!.ExpirationDate < DateTime.Now) throw new Exception("Your DeviseHR Subscription has ended");

            if (user.IsTerminated) throw new Exception("Your Permissions have been Revoked.");

            if (user.IsVerified == false) throw new Exception("Please sign into your account with the registration email that was sent to you. Please make sure to check the junk and spam folders. If you did not receive it, please contact your manager.");

            string? loginAtteptsAllowed = Environment.GetEnvironmentVariable("LOGIN_ATTEPTS_ALLOWED");

            if (loginAtteptsAllowed == null) throw new Exception("Environment Error");

            int loginAttemptsAllowed = int.Parse(loginAtteptsAllowed);

            if (user.LoginAttempt > loginAttemptsAllowed) throw new Exception("You have attempted to login multiple times unsuccessfully. Please contact your manager to regain access to your account.");
        }

        public static async Task VerifyPassword(User user, string clientPassword)
        {
            bool isMatch = BCrypt.Net.BCrypt.Verify(clientPassword, user.PasswordHash);

            if (!isMatch)
            {
                await EmployeeRepository.IncrementLoginAttepts(user);
                throw new Exception("Invalid Email or Password");
            }
        }

        public static string GenerateHash(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(); // Generate a random salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt); // Generate the hash

            return hashedPassword;
        }
    }
}
