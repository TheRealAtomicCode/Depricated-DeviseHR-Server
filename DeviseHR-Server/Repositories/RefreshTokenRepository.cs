﻿using DeviseHR_Server.Models;
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


        public static async Task RemoveRefreshTokenByUserId(int userId, string refreshToken)
        {
            using (var db = new DeviseHrContext())
            {
                User? user = await db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

                if (user == null) throw new Exception("Invalid user credencials");

                string? tokenToRemove = user.RefreshTokens.FirstOrDefault(rt => rt == refreshToken);

                if (tokenToRemove == null) throw new Exception("Please Authenticate");

                user.RefreshTokens.Remove(tokenToRemove);
                user.LastActiveTime = DateTime.Now;
                user.LastLoginTime = DateTime.Now;

                await db.SaveChangesAsync();
            }
        }

        public static async Task ClearRefreshTokensListByUserId(int userId)
        {
            using (var db = new DeviseHrContext())
            {
                User? user = await db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

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
