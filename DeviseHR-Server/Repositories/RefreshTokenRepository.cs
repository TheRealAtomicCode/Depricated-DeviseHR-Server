using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DeviseHR_Server.Repositories
{
    public class RefreshTokenRepository
    {
        public static async Task<User> UpdateRefreshTokensByUserId(int userId, string newRefreshToken, string oldRefreshToken)
        {
            using (var db = new DeviseHrContext())
            {
                User? user = await db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

                if (user == null) throw new Exception("Critical user Error");

                if (!string.IsNullOrEmpty(oldRefreshToken))
                {
                    string? tokenToRemove = user.RefreshTokens.FirstOrDefault(rt => rt == oldRefreshToken);

                    if (tokenToRemove != null)
                    {
                        user.RefreshTokens.Remove(tokenToRemove);
                    }
                }
                else if (user.RefreshTokens.Count > 6)
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
