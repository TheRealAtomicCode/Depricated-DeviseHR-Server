using DeviseHR_Server.Models;

namespace DeviseHR_Server.DTOs.RequestDTOs
{
    public class AddAbsenceRequest
    {
        public int UserId { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public bool? IsFirstHalfDay { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public int AbsenceTypes { get; set; }

        public string? Comments { get; set; }

        public bool IsApproved { get; set; } = false;


    }



}
