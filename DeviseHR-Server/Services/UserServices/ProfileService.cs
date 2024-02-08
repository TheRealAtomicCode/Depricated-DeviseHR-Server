using DeviseHR_Server.DTOs.ResponseDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.Design;

namespace DeviseHR_Server.Services.UserServices
{
    public class ProfileService
    {
        public static async Task<User> GetMyProfile(int userId) {
            User user = await UserRepository.GetUserById(userId);

            user.RefreshTokens.Clear();
            user.PasswordHash = "";
            return user;
        }

        public static async Task<List<FoundUser>> GetAllCompanyUsers(int myId, int companyId, int pageNo, int userType, bool enableShowEmployees)
        {
            List<FoundUser> users = await UserRepository.GetCompanyUsers(myId, companyId, pageNo, userType, enableShowEmployees);

            return users;
        }
    }
}
