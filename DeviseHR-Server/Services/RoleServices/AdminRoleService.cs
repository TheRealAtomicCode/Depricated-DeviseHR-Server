using DeviseHR_Server.Common;
using DeviseHR_Server.DTOs.ResponseDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.RoleRepositories;
using System.ComponentModel.Design;
using static DeviseHR_Server.DTOs.RequestDTOs.RoleRequests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviseHR_Server.Services.RoleServices
{
    public class AdminRoleService
    {

        public static async Task<Role> CreateRoleService(NewRole newRole, int myId, int companyId)
        {
            Filters.filterNewRoleData(newRole);

            Role role = await AdminRoleRepository.CreateRoleRepo(newRole, myId, companyId);
           
            return role;
        }

        public static async Task<UserAndRolesDto> GetUsersAndRolesService(int myId, int companyId)
        {
            return await AdminRoleRepository.GetUsersAndRolesRepo(myId, companyId);
        }




    }
}
