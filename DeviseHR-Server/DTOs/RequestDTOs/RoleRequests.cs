namespace DeviseHR_Server.DTOs.RequestDTOs
{
    public class RoleRequests
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
    }
}
