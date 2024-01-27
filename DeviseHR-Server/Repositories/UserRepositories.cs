using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

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

    }
}
