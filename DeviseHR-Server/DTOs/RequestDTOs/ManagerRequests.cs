﻿namespace DeviseHR_Server.DTOs.RequestDTOs
{
    public class ManagerRequests
    {

       
        public class NewUser
        {
            public string FirstName { get; set; } = String.Empty;
            public string LastName { get; set; } = String.Empty;
            public string Email { get; set; } = String.Empty;
            public int UserType { get; set; }
            public int? RoleId { get; set; }
            public DateOnly DateOfBirth { get; set; }
            public DateOnly? AnnualLeaveYearStartDate { get; set; }
        }

        public class TrimmedUserData
        {
            public string FirstName { get; set; } = String.Empty;
            public string LastName { get; set; } = String.Empty;
            public string Email { get; set; } = String.Empty;
        }


    }
}
