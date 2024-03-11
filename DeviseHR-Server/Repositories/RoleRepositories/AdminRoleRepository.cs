using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.DTOs.ResponseDTOs;
using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DeviseHR_Server.Repositories.RoleRepositories
{
    public class AdminRoleRepository
    {
        public static async Task<Role> CreateRoleRepo(RoleData newRole, int myId, int companyId)
        {
            var db = new DeviseHrContext();

            Role? foundRole = await db.Roles.Where(r => r.Name == newRole.Name && r.CompanyId == companyId).FirstOrDefaultAsync();

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


            await db.AddAsync(role);

            await db.SaveChangesAsync();

            return role;
        }

        public static async Task<Role> EditRoleRepo(RoleData roleData, int roleId, int myId, int companyId)
        {
            using var db = new DeviseHrContext();

            Role? roleToUpdate = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId && r.CompanyId == companyId);
            Role? roleWithSameName = await db.Roles.Where(r => r.Name == roleData.Name && r.CompanyId == companyId).FirstOrDefaultAsync();

            if (roleToUpdate == null) throw new Exception("Role not found");

            if (roleWithSameName != null)
            {
                if (roleData.Name == roleWithSameName.Name && roleToUpdate.Id != roleWithSameName.Id) throw new Exception("Role name already exists");
            }

            roleToUpdate.Name = roleData.Name;
            roleToUpdate.EnableAddEmployees = roleData.EnableAddEmployees;
            roleToUpdate.EnableAddLateness = roleData.EnableAddLateness;
            roleToUpdate.EnableApproveAbsence = roleData.EnableApproveAbsence;
            roleToUpdate.EnableAddManditoryLeave = roleData.EnableAddManditoryLeave;
            roleToUpdate.EnableCreatePattern = roleData.EnableCreatePattern;
            roleToUpdate.EnableCreateRotas = roleData.EnableCreateRotas;
            roleToUpdate.EnableDeleteEmployee = roleData.EnableDeleteEmployee;
            roleToUpdate.EnableTerminateEmployees = roleData.EnableTerminateEmployees;
            roleToUpdate.EnableViewEmployeeNotifications = roleData.EnableViewEmployeeNotifications;
            roleToUpdate.EnableViewEmployeePayroll = roleData.EnableViewEmployeePayroll;
            roleToUpdate.EnableViewEmployeeSensitiveInformation = roleData.EnableViewEmployeeSensitiveInformation;
            roleToUpdate.UpdatedAt = new DateTime();
            roleToUpdate.UpdatedBy = myId;

            await db.SaveChangesAsync();

            return roleToUpdate;
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



        public static async Task EditUserTypesRepo(List<UsersRoles> usersRoles, int myId, int companyId)
        {
            var db = new DeviseHrContext();

            List<int> existingRoleIds = await db.Roles
                .Select(r => r.Id)
                .ToListAsync();

            List<int> userIds = usersRoles.Select(ur => ur.UserId).ToList();

            List<User> existingUsers = await db.Users
                .Include(u => u.HierarchyManagers)
                .Include(u => u.HierarchySubordinates)
                .Where(u => userIds.Contains(u.Id) && u.CompanyId == companyId)
                .ToListAsync();

            if (existingUsers.Count != usersRoles.Count) throw new Exception("An Error occured while retrieving the users roles");

            for (int i = 0; i < existingUsers.Count; i++)
            {
                var retrievedUser = usersRoles[i];
                var existingUser = existingUsers[i];

                if (myId == retrievedUser.UserId) throw new Exception("Can not change your own Role");

                if (retrievedUser.UserType != 1 && retrievedUser.UserType != 2 && retrievedUser.UserType != 3) throw new Exception("Invalid User Type provided");

                if (retrievedUser.UserType == 1 || retrievedUser.UserType == 3)
                {
                    existingUser.HierarchyManagers.Clear();
                    existingUser.UserType = retrievedUser.UserType;
                    existingUser.RoleId = null;
                    existingUser.UpdatedAt = DateTime.Now;
                    existingUser.UpdatedByUser = myId;
                }
                else
                {
                    if (retrievedUser.RoleId == null) throw new Exception("Role not specified");

                    if (!existingRoleIds.Contains((int)retrievedUser.RoleId)) throw new Exception("Role ID does not exist.");

                    existingUser.HierarchySubordinates.Clear();
                    existingUser.UserType = 2;
                    existingUser.RoleId = retrievedUser.RoleId;
                    existingUser.UpdatedAt = DateTime.Now;
                    existingUser.UpdatedByUser = myId;
                }

            }

            await db.SaveChangesAsync();

        }



        public static async Task EditSubordinatesRepo(ManagersAndSubordinates managersAndSubordinates, int myId, int companyId)
        {
            var db = new DeviseHrContext();


            // Connect to the database
            await db.Database.OpenConnectionAsync();


            // Create a raw SQL query
            var query = "SELECT * FROM edit_subordinates({0}, {1}, {2}, {3}, {4}, {5})";
            var parameters = new object[]
            {
                managersAndSubordinates.ManagersToBeAdded,
                managersAndSubordinates.SubordinatesToBeAdded,
                managersAndSubordinates.ManagersToBeRemoved,
                managersAndSubordinates.SubordinatesToBeRemoved,
                myId,
                companyId
            };

            // Execute the raw SQL query
            await db.Database.ExecuteSqlRawAsync(query, parameters);
        }



        public static async Task<List<RetrievedSubordinates>> GetSubordinatesByManagerIdRepo(int managerId, int companyId)
        {
            var db = new DeviseHrContext();

            var users = await db.Users.ToListAsync();

            var userInfos = (
                from u in users
                join h in db.Hierarchies on u.Id equals h.SubordinateId into subordinates
                from s in subordinates.DefaultIfEmpty()
                where u.CompanyId == companyId && u.Id != managerId
                select new RetrievedSubordinates
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserType = u.UserType,
                    Id = u.Id,
                    IsSubordinate = (s != null && s.ManagerId == managerId)
                }
            ).ToList();

            return userInfos;
        }


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
