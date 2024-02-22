using DeviseHR_Server.Helpers;
using static DeviseHR_Server.DTOs.RequestDTOs.ManagerUserRequests;
using static DeviseHR_Server.DTOs.RequestDTOs.RoleRequests;

namespace DeviseHR_Server.Common
{
    public class Filters
    {
        public static void filterNewUserData(NewUser user)
        {
            user.FirstName = user.FirstName.Trim();
            user.LastName = user.LastName.Trim();
            user.Email = user.Email.ToLower().Trim();
            StringValidation.ValidateNonEmptyStrings([user.FirstName, user.LastName, user.Email]);
            StringValidation.ValidateEmail(user.Email);
        }

        public static void filterNewRoleData(NewRole role)
        {
            role.Name = role.Name.Trim();
            StringValidation.ValidateNonEmptyStrings([role.Name]);
        }
    }
}
