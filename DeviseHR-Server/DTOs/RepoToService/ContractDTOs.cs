using DeviseHR_Server.Models;

namespace DeviseHR_Server.DTOs.RepoToServiceDTOs
{
    public class CalculateLeaveYearDtoFromRepoToService
    {
        public List<LeaveYear> LeaveYears { get; set; } = new List<LeaveYear>();
        public LeaveYear? CurrentLeaveYear { get; set; }
        public User User { get; set; }
        public List<Contract> Contracts { get; set; } = new List<Contract>();
    }



}


