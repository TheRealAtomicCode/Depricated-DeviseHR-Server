﻿namespace DeviseHR_Server.DTOs.RequestDTOs
{

    public class RoleData
    {
        public string Name { get; set; } = null!;
        public bool EnableAddEmployees { get; set; }
        public bool EnableTerminateEmployees { get; set; }
        public bool EnableDeleteEmployee { get; set; }
        public bool EnableCreatePattern { get; set; }
        public bool EnableApproveAbsence { get; set; }
        public bool EnableAddManditoryLeave { get; set; }
        public bool EnableAddLateness { get; set; }
        public bool EnableCreateRotas { get; set; }
        public bool EnableViewEmployeeNotifications { get; set; }
        public bool EnableViewEmployeePayroll { get; set; }
        public bool EnableViewEmployeeSensitiveInformation { get; set; }
    }

    public class UsersRoles
    {
        public int UserId { get; set; }
        public int UserType { get; set; }
        public int? RoleId { get; set; }
    }

    public class Subordinates
    {
        public List<int> ManagersToBeAdded { get; set; } = new List<int>();
        public List<int> SubordinatesToBeAdded { get; set; } = new List<int>();
        public List<int> ManagersToBeRemoved { get; set; } = new List<int>();
        public List<int> SubordinatesToBeRemoved { get; set; } = new List<int>();
    }

}

