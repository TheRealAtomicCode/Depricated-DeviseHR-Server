using DeviseHR_Server.DTOs.ResponseDTOs;
using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviseHR_Server.Repositories.RoleRepositories
{
    public static class ManagerRoleRepository
    {
        
        public static async Task<List<Role>> GetCompanyRolesByIdRepo(int companyId)
        {
            var db = new DeviseHrContext();

            List<Role> companyRoles = await db.Roles
                .Where(r => r.CompanyId == companyId)
                .ToListAsync();

            return companyRoles;
        }


    }
}
