using DeviseHR_Server.DTOs.ResponseDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.RoleRepositories;

namespace DeviseHR_Server.Services.RoleServices
{
    public class ManagerRoleService
    {
        
        public static async Task<List<Role>> GetExistingRolesService(int companyId)
        {
            return await ManagerRoleRepository.GetCompanyRolesByIdRepo(companyId);
        }
    }
}
