namespace DeviseHR_Server.DTOs.RequestDTOs
{
    public class ManagerUserRequests
    {
        public class NewUser
        {
            public string FirstName { get; set; } = String.Empty;
            public string LastName { get; set; } = String.Empty;
            public string Email { get; set; } = String.Empty;
            public int UserType { get; set; }
            public int? RoleId { get; set; }
            public bool RegisterUser { get; set; } = false;
            public DateOnly DateOfBirth { get; set; }
            public DateOnly? AnnualLeaveYearStartDate { get; set; }
        }


    }
}
