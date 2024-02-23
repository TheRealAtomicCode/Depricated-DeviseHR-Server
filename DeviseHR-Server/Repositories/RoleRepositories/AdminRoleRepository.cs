using DeviseHR_Server.DTOs.ResponseDTOs;
using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;
using static DeviseHR_Server.DTOs.RequestDTOs.RoleRequests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviseHR_Server.Repositories.RoleRepositories
{
    public class AdminRoleRepository
    {
        public static async Task<Role> CreateRoleRepo(RoleData newRole, int myId, int companyId)
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

        public static async Task<Role> EditRoleRepo(RoleData roleData, int roleId, int myId, int companyId)
        {
            using var db = new DeviseHrContext();

            Role? foundRole = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId && r.CompanyId == companyId);

            if (foundRole == null) throw new Exception("Role not found");

            foundRole.Name = roleData.Name;
            foundRole.EnableAddEmployees = roleData.EnableAddEmployees;
            foundRole.EnableAddLateness = roleData.EnableAddLateness;
            foundRole.EnableApproveAbsence = roleData.EnableApproveAbsence;
            foundRole.EnableAddManditoryLeave = roleData.EnableAddManditoryLeave;
            foundRole.EnableCreatePattern = roleData.EnableCreatePattern;
            foundRole.EnableCreateRotas = roleData.EnableCreateRotas;
            foundRole.EnableDeleteEmployee = roleData.EnableDeleteEmployee;
            foundRole.EnableTerminateEmployees = roleData.EnableTerminateEmployees;
            foundRole.EnableViewEmployeeNotifications = roleData.EnableViewEmployeeNotifications;
            foundRole.EnableViewEmployeePayroll = roleData.EnableViewEmployeePayroll;
            foundRole.EnableViewEmployeeSensitiveInformation = roleData.EnableViewEmployeeSensitiveInformation;

            await db.SaveChangesAsync();

            return foundRole;
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
