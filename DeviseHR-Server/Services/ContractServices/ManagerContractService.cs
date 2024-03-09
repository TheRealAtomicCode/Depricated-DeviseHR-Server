

using DeviseHR_Server.DTOs.RequestDTOs;
using DeviseHR_Server.Models;
using DeviseHR_Server.Repositories.ContractRepositories;
using DeviseHR_Server.Repositories.UserRepository;

namespace DeviseHR_Server.Services.ContractServices
{
    public class ManagerContractService
    {

        public static async Task<Contract> AddContract(CreateContractRequest newContract, int myId, int companyId, int userType)
        {

            if (userType > 1 && newContract.UserId == myId) throw new Exception("Managers can not end their own contracts");

            DateOnly startDate;
            DateOnly? endDate;

            if (!DateOnly.TryParse(newContract.StartDate, out startDate))
            {
                throw new ArgumentException("Invalid date format for StartDate.");
            }

            if (!DateOnly.TryParse(newContract.EndDate, out var parsedEndDate))
            {
                endDate = null;
            }
            else
            {
                endDate = parsedEndDate;
            }

            var contract = new Contract
            {
                UserId = newContract.UserId,
                PatternId = newContract.PatternId,
                ContractType = newContract.ContractType,
                StartDate = startDate,
                EndDate = endDate,
                ContractedHoursPerWeekInMinutes = newContract.ContractedHoursPerWeekInMinutes,
                CompanyHoursPerWeekInMinutes = newContract.CompanyHoursPerWeekInMinutes,
                ContractedDaysPerWeekInHalfs = newContract.ContractedDaysPerWeekInHalfs,
                CompanyDaysPerWeekInHalfs = newContract.CompanyDaysPerWeekInHalfs,
                AverageWorkingDay = newContract.AvrageWorkingDay,
                IsLeaveInDays = newContract.IsLeaveInDays,
                CompanyLeaveEntitlement = newContract.CompanyLeaveEntitlement,
                ContractedLeaveEntitlement = newContract.ContractedLeaveEntitlement,
                ThisContractsLeaveAllowence = newContract.ThisContractsLeaveAllowence,
                TermTimeId = newContract.TermTimeId,
                AddedBy = myId,
                UpdatedBy = 0,
                CompanyId = companyId
            };

            Contract addedContract = await ManageContractRepository.AddContract(contract);
            return addedContract;
        }



        public static async Task<CreateContractRequest> CalculateLeaveYear(int userId, int myId, int companyId, int userType, CreateContractRequest contract)
        {

            if (contract.ContractType == 1) throw new Exception("Contract does not require calculation");

            if (contract.ContractType == 3) throw new Exception("Working Patterns need to be added first by the developers");

            if (contract.TermTimeId != null) throw new Exception("Term times need to be added first by the developers");

            int previousTotalLeaveEntitlement = 0;
            DateTime? leaveYearStartDate = null;
            DateTime? leaveYearEndDate = null;


            if (contract.ContractType == 2)
            {

                var leaveYears = await ManageContractRepository.GetLeaveYearRepo(userId, companyId, myId, false);

                if (contract.IsLeaveInDays == true && (contract.ContractedDaysPerWeekInHalfs == null || contract.CompanyDaysPerWeekInHalfs == null))
                {
                    throw new Exception("can not perform calculation for days");
                }

                if (contract.IsLeaveInDays == false && (contract.ContractedHoursPerWeekInMinutes != null && contract.CompanyHoursPerWeekInMinutes != null))
                {
                    throw new Exception("can not perform calculation for hours");
                }


                if (leaveYears.Count > 0)
                {
                    previousTotalLeaveEntitlement = leaveYears[leaveYears.Count - 1].TotalLeaveEntitlement;
                    leaveYearStartDate = leaveYears[leaveYears.Count - 1].LeaveYearStartDate;
                }
                else
                {
                    var user = await EmployeeRepository.GetUserById(userId, companyId);

                    if (user != null)
                    {
                        if (user.AnnualLeaveStartDate != null)
                        {
                            DateOnly userLeaveYearStartDate = user.AnnualLeaveStartDate;
                            leaveYearStartDate = userLeaveYearStartDate.ToDateTime(TimeOnly.MinValue);
                        }
                        else
                        {
                            DateOnly companyLeaveYearStartDate = (DateOnly)user.Company.AnnualLeaveStartDate;
                            leaveYearStartDate = companyLeaveYearStartDate.ToDateTime(TimeOnly.MinValue);

                        }
                    }

                }

                if (contract.EndDate == null)
                {
                    leaveYearEndDate = (DateTime)leaveYearStartDate?.AddYears(1);
                }
                else
                {
                    string endDateString = contract.EndDate;
                    leaveYearEndDate = DateTime.Parse(endDateString);
                }


                if (contract.IsLeaveInDays == true)
                {
                    double annualLeaveEntitlement = (contract.ContractedDaysPerWeekInHalfs / contract.CompanyDaysPerWeekInHalfs) * contract.CompanyLeaveEntitlement;

                    TimeSpan duration = (DateTime)leaveYearEndDate?.Date - (DateTime)leaveYearStartDate?.Date;
                    int daysBetweenContractStartDateAndEndDate = duration.Days;

                    int contractedLeave = (int)Math.Ceiling((double)annualLeaveEntitlement * daysBetweenContractStartDateAndEndDate / 365);

                    contract.ContractedLeaveEntitlement = previousTotalLeaveEntitlement + contractedLeave;
                }
                if (contract.IsLeaveInDays == false)
                {
                    double annualLeaveEntitlement = (contract.ContractedHoursPerWeekInMinutes / contract.CompanyHoursPerWeekInMinutes) * contract.CompanyLeaveEntitlement;

                    TimeSpan duration = (DateTime)leaveYearEndDate?.Date - (DateTime)leaveYearStartDate?.Date;
                    int daysBetweenContractStartDateAndEndDate = duration.Days;

                    int contractedLeave = (int)Math.Ceiling((double)annualLeaveEntitlement * daysBetweenContractStartDateAndEndDate / 365);

                    contract.ContractedLeaveEntitlement = previousTotalLeaveEntitlement + contractedLeave;
                }


            }

                return contract;

        }


        public static async Task EndLastContractService(int userId, string endDate, int myId, int companyId, int userType)
        {
            if (userType > 1 && userId == myId) throw new Exception("Managers can not end their own contracts");

            await ManageContractRepository.EndLastContractRepo(userId, endDate, myId, companyId, userType);

        }


        public static async Task GetLeaveYearService(int userId, int myId, int companyId, int userType, bool enableShowEmployees)
        {
            bool checkIfSubordinate = true;
            if ((userType == 2 && enableShowEmployees == true) || userType == 1) checkIfSubordinate = false;

            ManageContractRepository.GetLeaveYearRepo(userId, companyId, myId, checkIfSubordinate);

        }




    }
}
