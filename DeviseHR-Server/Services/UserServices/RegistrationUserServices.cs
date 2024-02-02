using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs;
using DeviseHR_Server.Helpers;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories;

namespace DeviseHR_Server.Services.UserServices
{
    public class RegistrationUserServices
    {
        public static async Task<ServiceResponse<User>> LoginUserByCredencials(string email, string password)
        {
            User user = await UserRepository.GetUserByEmail(email.Trim());

            AccessMethods.VerifyPassword(user, password);

            AccessMethods.VerifyUserAccess(user);

            string token = await Tokens.GenerateUserAuthJWT(user);

            string refreshToken = await Tokens.GenerateUserRefreshToken(user);

            await RefreshTokenRepository.UpdateRefreshTokensByUserId(user.Id, refreshToken, "");

            user.PasswordHash = string.Empty;
            user.RefreshTokens.Clear();

            var serviceResponse = new ServiceResponse<User>(user, true, "", token, refreshToken);
    
            return serviceResponse;
        }

        public static async Task<ServiceResponse<User>> RefreshUserToken(int userId, string refreshToken)
        {
            User user = await UserRepository.GetUserByIdAndRefreshToken(userId, refreshToken);

            AccessMethods.VerifyUserAccess(user);

            string token = await Tokens.GenerateUserAuthJWT(user);

            string newRefreshToken = await Tokens.GenerateUserRefreshToken(user);

            await RefreshTokenRepository.UpdateRefreshTokensByUserId(user.Id, newRefreshToken, refreshToken);

            user.PasswordHash = string.Empty;
            user.RefreshTokens.Clear();

            var serviceResponse = new ServiceResponse<User>(user, true, "", token, newRefreshToken);

            return serviceResponse;
        }

        public static async Task LogoutUserByRefreshToken(int userId, string refreshToken)
        {
            await RefreshTokenRepository.RemoveRefreshTokenByUserId(userId, refreshToken);
        }

        public static async Task LogoutAllDevicesByUserId(int userId)
        {
            await RefreshTokenRepository.ClearRefreshTokensListByUserId(userId);
        }

        public static async Task resetPasswordByEmail(string email)
        {
            string verificationCode = StringGeneration.GenerateSixDigitString();

            User user = await UserRepository.GetUserByEmail(email.Trim());

            AccessMethods.VerifyUserAccess(user);

            await UserRepository.UpdateUserVerificationCodeById(user.Id, verificationCode);
        }
    }
}
