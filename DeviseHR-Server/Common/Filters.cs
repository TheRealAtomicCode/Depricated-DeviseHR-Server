using DeviseHR_Server.Helpers;
using static DeviseHR_Server.DTOs.RequestDTOs.ManagerRequests;

namespace DeviseHR_Server.Common
{
    public class Filters
    {
        public static TrimmedUserData filternewUserData(string firstName, string lastName, string email)
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();
            email = email.ToLower().Trim();
            StringValidation.ValidateNonEmptyStrings([firstName, lastName, email]);
            StringValidation.ValidateEmail(email);
            
            TrimmedUserData trimmedUser = new TrimmedUserData();
            trimmedUser.FirstName = firstName;
            trimmedUser.LastName = lastName;
            trimmedUser.Email = email;
            
            return trimmedUser;
        }
    }
}
