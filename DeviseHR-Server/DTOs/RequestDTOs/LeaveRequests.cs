using DeviseHR_Server.Models;

namespace DeviseHR_Server.DTOs.RequestDTOs
{
    public class AddAbsenceRequest
    {
        public int UserId { get; set; }

        public string StartDate { get; set; } = "";

        public string EndDate { get; set; } = "";

        public bool? IsFirstHalfDay { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public int AbsenceTypes { get; set; }

        public string? Comments { get; set; }

        public bool IsApproved { get; set; } = false;


    }



}
