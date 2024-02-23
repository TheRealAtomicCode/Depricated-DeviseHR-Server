using DeviseHR_Server.Models;

namespace DeviseHR_Server.DTOs.ResponseDTOs
{

    public class UserPermissionDetails
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public int UserType { get; set; }
        public int? RoleId { get; set; }
    }

    public class UserAndRolesDto
    {
        public List<UserPermissionDetails> Users = new List<UserPermissionDetails>();
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}

