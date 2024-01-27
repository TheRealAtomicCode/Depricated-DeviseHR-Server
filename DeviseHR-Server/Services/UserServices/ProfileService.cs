using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories;

namespace DeviseHR_Server.Services.UserServices
{
    public class ProfileService
    {
        public static async Task<User> GetMyProfile(int userId) {
            User user = await UserRepository.GetUserById(userId);

            user.RefreshTokens.Clear();
            user.PasswordHash = "";

            return user;
        }
    }
}
