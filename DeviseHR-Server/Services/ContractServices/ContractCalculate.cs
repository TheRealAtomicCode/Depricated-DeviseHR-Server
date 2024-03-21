using System.Globalization;

namespace DeviseHR_Server.Services.ContractServices
{
    public class ContractCalculate
    {
        public static double CalculatePreviousLeave(string fromDate, string toDate, double previousLeaveEntitlement)
        {
            DateTime fromDateTime = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime toDateTime = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            int numberOfDays = toDateTime.Subtract(fromDateTime).Days;

            double result = previousLeaveEntitlement * ((double)numberOfDays / 365);

            return result;
        }

        public static double CalculateLeave(string fromDate, string toDate, int companyLeaveEntitlement, int employeeWorkingTime, int comanyWorkingTime)
        {
            double contractedToCompanyRatio = (double)employeeWorkingTime / (double)comanyWorkingTime;
            double contractedLeaveEntitlementInAYear = (double)companyLeaveEntitlement * contractedToCompanyRatio;

            DateTime fromDateTime = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime toDateTime = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            int numberOfDays = toDateTime.Subtract(fromDateTime).Days;

            double result = contractedLeaveEntitlementInAYear * ((double)numberOfDays / 365);

            return result;
        }

        
        public static double CalculateLeaveForFullYearOnContract(int companyLeaveEntitlement, int employeeWorkingTime, int comanyWorkingTime)
        {
            double contractedToCompanyRatio = (double)employeeWorkingTime / (double)comanyWorkingTime;
            double contractedLeaveEntitlementInAYear = (double)companyLeaveEntitlement * contractedToCompanyRatio;

            return contractedLeaveEntitlementInAYear;
        }
    }
}
