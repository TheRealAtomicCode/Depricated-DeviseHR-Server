using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviseHR_Server.Repositories.ContractRepositories
{
    public class ManageContractRepository
    {

        public static async Task<Contract> AddContract(Contract contract)
        {
            var db = new DeviseHrContext();

            db.Contracts.Add(contract);
            await db.SaveChangesAsync();

            return contract;
        }

        
        public static async Task EndLastContractRepo(int userId, string endDate, int myId, int companyId, int userType)
        {
            using (var dbContext = new DeviseHrContext()) 
            {
                await dbContext.Database.ExecuteSqlRawAsync("SELECT * FROM update_last_contract_end_date({0}, {1}, {2}, {3}, {4})", userId, myId, companyId, endDate, userType);
            }
        }



    }
}
