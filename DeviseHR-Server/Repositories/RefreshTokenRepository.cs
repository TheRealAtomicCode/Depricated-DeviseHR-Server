using DeviseHR_Server.Models;

namespace DeviseHR_Server.Repositories
{
    public class RefreshTokenRepository
    {
        public static async Task<User> UpdateRefreshTokensByUserId(User user, string newRefreshToken)
        {
            using (var db = new DeviseHrContext())
            {
                if (user.RefreshTokens.Count > 6)
                {
                    user.RefreshTokens.Clear();
                }

                user.RefreshTokens.Add(newRefreshToken);
                
                user.LastActiveTime = DateTime.Now;
                user.LastLoginTime = DateTime.Now;

                await db.SaveChangesAsync();

                return user;
            }
        }
    }
}
