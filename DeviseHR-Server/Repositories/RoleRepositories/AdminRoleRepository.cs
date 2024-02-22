using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;
using static DeviseHR_Server.DTOs.RequestDTOs.RoleRequests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviseHR_Server.Repositories.RoleRepositories
{
    public class AdminRoleRepository
    {
        public static async Task<Role> CreateRole(NewRole newRole, int myId, int companyId)
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
    }
}
