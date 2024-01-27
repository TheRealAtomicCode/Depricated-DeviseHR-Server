using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories;

namespace DeviseHR_Server.Services.UserServices
{
    public class RegistrationUserServices
    {
        public static async Task<ServiceResponse<User>> GetUserByCredencials(string email, string password)
        {
            User user = await UserRepository.GetUserByEmail(email.Trim());

            AccessMethods.VerifyPassword(user, password);

            AccessMethods.VerifyUserAccess(user);

            string token = await Tokens.GenerateUserAuthJWT(user);

            string refreshToken = await Tokens.GenerateUserRefreshToken(user);

            User loggedInUser = await RefreshTokenRepository.UpdateRefreshTokensByUserId(user, refreshToken);

            loggedInUser.PasswordHash = string.Empty;
            loggedInUser.RefreshTokens.Clear();

            var serviceResponse = new ServiceResponse<User>(loggedInUser, true, "", token, refreshToken);
    
            return serviceResponse;
        }
    }
}
