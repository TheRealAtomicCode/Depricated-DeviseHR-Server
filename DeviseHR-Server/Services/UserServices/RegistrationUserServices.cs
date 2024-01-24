using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories;

namespace DeviseHR_Server.Services.UserServices
{
    public class RegistrationUserServices
    {
        public static async Task<ServiceResponse<User>> LoginUser(string email, string password)
        {
            var user = await UserRepository.GetUserByCredencials(email.Trim(), password.Trim());

            bool isMatch = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isMatch)
            {
                throw new Exception("Invalid Email or Password");
            }

           
            if (user.Company!.ExpirationDate < DateTime.Now) throw new Exception("Your DeviseHR Subscription has ended");

            AccessMethods.VerifyAccess(user.IsVerified, user.IsTerminated, (int)user.LoginAttempt!);

            string token = Tokens.GenerateUserAuthJWT(user);

            var serviceResponse = new ServiceResponse<User>(user, true, "", token!);
    
            return serviceResponse;
        }
    }
}
