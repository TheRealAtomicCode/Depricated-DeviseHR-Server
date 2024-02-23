using DeviseHR_Server.DTOs.ResponseDTOs;
using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;
using static DeviseHR_Server.DTOs.RequestDTOs.RoleRequests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviseHR_Server.Repositories.RoleRepositories
{
    public class AdminRoleRepository
    {
        public static async Task<Role> CreateRoleRepo(NewRole newRole, int myId, int companyId)
        {
            var db = new DeviseHrContext();

            Role? foundRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == newRole.Name && r.CompanyId == companyId);

            if (foundRole != null) throw new Exception("Role name already exists");

            Role role = new Role();

            role.Name = newRole.Name;
            role.CompanyId = companyId;
            role.AddedBy = myId;

            role.EnableAddEmployees = newRole.EnableAddEmployees;
            role.EnableAddLateness = newRole.EnableAddLateness;
            role.EnableApproveAbsence = newRole.EnableApproveAbsence;
            role.EnableAddManditoryLeave = newRole.EnableAddManditoryLeave;
            role.EnableCreatePattern = newRole.EnableCreatePattern;
            role.EnableCreateRotas = newRole.EnableCreateRotas;
            role.EnableDeleteEmployee = newRole.EnableDeleteEmployee;
            role.EnableTerminateEmployees = newRole.EnableTerminateEmployees;
            role.EnableViewEmployeeNotifications = newRole.EnableViewEmployeeNotifications;
            role.EnableViewEmployeePayroll = newRole.EnableViewEmployeePayroll;
            role.EnableViewEmployeeSensitiveInformation = newRole.EnableViewEmployeeSensitiveInformation;

            await db.AddAsync(newRole);

            return role;
        }

        
        public static async Task<UserAndRolesDto> GetUsersAndRolesRepo(int myId, int companyId)
        {
            var db = new DeviseHrContext();

            List<Role> companyRoles = await db.Roles
                .Where(r => r.CompanyId == companyId)
                .ToListAsync();

            List<UserPermissionDetails> users = await db.Users
                .Where(u => u.CompanyId == myId)
                .Select(u => new UserPermissionDetails
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        UserType = u.UserType,
                        RoleId = u.RoleId,
                    })
                .ToListAsync();

            UserAndRolesDto userAndRolesDto = new UserAndRolesDto
            {
                Roles = companyRoles,
                Users = users
            };

            return userAndRolesDto;
        }
    }
}
