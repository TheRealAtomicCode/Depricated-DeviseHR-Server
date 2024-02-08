using DeviseHR_Server.Models;
using static DeviseHR_Server.DTOs.RequestDTOs.ManagerRequests;
using System.ComponentModel.Design;

namespace DeviseHR_Server.Repositories.UserRepository
{
    public class ManagerRepository
    {
        public static async Task<User> AddUser(NewUser newUser, int myId, int companyId)
        {
            User user = new User
            {
                CompanyId = companyId,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                DateOfBirth = newUser.DateOfBirth,
                UserType = newUser.UserType,
                AddedByUser = myId,
                AddedByOperator = 0,
                RoleId = newUser.RoleId == 2 ? newUser.RoleId : null,
                AnnualLeaveStartDate = newUser.AnnualLeaveYearStartDate // make the annual leave start date a DateOnly Type
            };

            using (var db = new UserRepository())
            {
                db.Users.Add(user);

                if (newUser.UserType != 1)
                {
                    Hierarchy hierarchy = new Hierarchy 
                    { 
                        ManagerId = myId,
                        SubordinateId = user.Id
                    };

                    db.Hierarchies.Add(hierarchy);
                        
                }
            
                db.SaveChanges();
            }

            return user;
            
        }
    
    }


}
}
