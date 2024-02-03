using DeviseHR_Server.Common;
using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace DeviseHR_Server.Repositories
{
    public class UserRepository
    {
        public static async Task<User> GetUserByEmail(string email)
        {
          
                using (var db = new DeviseHrContext())
                {
                    var user = await db.Users.Where(u => u.Email == email)
                        .Include(u => u.Company)
                        .Include(u => u.Role).FirstOrDefaultAsync();

                    if (user == null)
                    {
                        throw new Exception("Invalid Email or Password");
                    }

                    return user;
                }
           
        }


        public static async Task<User> GetUserById(int userId)
        {

            using (var db = new DeviseHrContext())
            {
                var user = await db.Users.Where(u => u.Id == userId)
                    .Include(u => u.Company)
                    .Include(u => u.Role).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("Invalid Email or Password");
                }

                return user;
            }

        }


        public static async Task<User> GetUserByIdAndRefreshToken(int userId, string refreshToken)
        {
            using (var db = new DeviseHrContext())
            {
                var user = await db.Users.Where(u => u.Id == userId && u.RefreshTokens.Any(rt => rt == refreshToken))
                    .Include(u => u.Company)
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("Please Authenticate");
                }

                return user;
            }
        }


        public static async Task UpdateUserVerificationCodeById(int userId, string verificationCode)
        {
            using (var db = new DeviseHrContext())
            {
                User? user = await db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

                if (user == null) throw new Exception("Invalid user credencials");

                user.VerificationCode = verificationCode;
                user.LastActiveTime = DateTime.Now;

                await db.SaveChangesAsync();
            }
        }

        public static async Task IncrementLoginAttepts(User user)
        {
            using (var db = new DeviseHrContext())
            {
                db.Users.Update(user);

                user.LoginAttempt++;

                await db.SaveChangesAsync();
            }
        }


        public static async Task ConfermVerificationCodeUpdatePasswordAndLoginUser(User user, string verificationCode, string newPassword, string refreshToken)
        {
            using (var db = new DeviseHrContext())
            {
                db.Users.Update(user);

                if (user.VerificationCode != verificationCode)
                {
                    await IncrementLoginAttepts(user);
                    throw new Exception("Invalid verification code");
                }

                user.VerificationCode = null;
                user.LastActiveTime = DateTime.Now;
                user.LastLoginTime = DateTime.Now;
                user.PasswordHash = AccessMethods.GenerateHash(newPassword);
                user.RefreshTokens.Add(refreshToken);
                user.LoginAttempt = 0;

                await db.SaveChangesAsync();
            }



          
        }

    }
}
