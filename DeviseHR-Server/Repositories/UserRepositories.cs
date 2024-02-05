using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs.ResponseDTOs;
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


        public static async Task<List<FoundUser>> GetCompanyUsersById(int myId, int companyId, int pageNo, int userType, bool enableShowEmployees)
        {

            if (enableShowEmployees)
            {
                if (userType == 3)
                {
                    throw new Exception("Not allowed to view users");
                }
                else if (userType == 2)
                {
                    using (var db = new DeviseHrContext())
                    {

                        var users = await (
                            from u in db.Users
                            join h in db.Hierarchies on u.Id equals h.ManagerId
                            where h.ManagerId == myId && u.CompanyId == companyId
                            select new FoundUser
                            {
                                Id = u.Id,
                                CompanyId = u.CompanyId,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Email = u.Email,
                                UserType = u.UserType,
                            })
                            .ToListAsync();

                        return users;
                    }
                }
                else
                {
                    using (var db = new DeviseHrContext())
                    {
                            var users = await (
                                from u in db.Users
                                where u.CompanyId == companyId
                                select new FoundUser
                                {
                                    Id = u.Id,
                                    CompanyId = u.CompanyId,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Email = u.Email,
                                    UserType = u.UserType,
                                })
                                .ToListAsync();

                            return users;
                    }
                }
                



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
