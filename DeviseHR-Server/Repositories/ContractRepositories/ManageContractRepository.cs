using DeviseHR_Server.Models;

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
    }
}
