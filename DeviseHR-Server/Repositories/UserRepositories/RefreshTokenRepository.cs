using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DeviseHR_Server.Repositories.UserRepository.UserRepository
{
    public class RefreshTokenRepository
    {
        public static async Task<User> UpdateRefreshTokensByUserId(User user, string newRefreshToken, string oldRefreshToken)
        {
            using (var db = new DeviseHrContext())
            {
                db.Update(user);

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
                user.LoginAttempt = 0;
                user.LastActiveTime = DateTime.Now;
                user.LastLoginTime = DateTime.Now;

                await db.SaveChangesAsync();

                return user;
            }
        }


        public static async Task RemoveRefreshTokenByUserId(int userId, int companyId, string refreshToken)
        {
            using (var db = new DeviseHrContext())
            {
                User? user = await db.Users.Where(u => u.Id == userId && u.CompanyId == companyId).FirstOrDefaultAsync();

                if (user == null) throw new Exception("Invalid user credencials");

                string? tokenToRemove = user.RefreshTokens.FirstOrDefault(rt => rt == refreshToken);

                if (tokenToRemove == null) throw new Exception("Please Authenticate");

                user.RefreshTokens.Remove(tokenToRemove);
                user.LastActiveTime = DateTime.Now;
                user.LastLoginTime = DateTime.Now;

                await db.SaveChangesAsync();
            }
        }

        public static async Task ClearRefreshTokensListByUserId(int userId, int companyId)
        {
            using (var db = new DeviseHrContext())
            {
                User? user = await db.Users.Where(u => u.Id == userId && u.CompanyId == companyId).FirstOrDefaultAsync();

                if (user == null) throw new Exception("Invalid user credencials");

                if (user.RefreshTokens.Count <= 0) throw new Exception("Please Authenticate");

                user.RefreshTokens.Clear();
                user.LastActiveTime = DateTime.Now;
                user.LastLoginTime = DateTime.Now;

                await db.SaveChangesAsync();
            }
        }


    }
}
