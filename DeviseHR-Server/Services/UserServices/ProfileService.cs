using DeviseHR_Server.DTOs.ResponseDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.UserRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.Design;

namespace DeviseHR_Server.Services.UserServices
{
    public class ProfileService
    {
        public static async Task<User> GetMyProfile(int userId, int companyId)
        {
            User user = await EmployeeRepository.GetUserById(userId, companyId);

            user.RefreshTokens.Clear();
            user.PasswordHash = "";
            return user;
        }

        public static async Task<List<FoundUser>> GetAllCompanyUsers(int myId, int companyId, int pageNo, int userType, bool enableShowEmployees)
        {
            List<FoundUser> users = await EmployeeRepository.GetCompanyUsers(myId, companyId, pageNo, userType, enableShowEmployees);

            return users;
        }
    }
}
