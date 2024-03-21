using DeviseHR_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviseHR_Server.Repositories.UserRepositories
{
    public class UserRelationsRepo
    {
        public static async Task<bool> IsRelated(int managerId, int subordinateId)
        {
            var db = new DeviseHrContext();

            var hierarchy = await db.Hierarchies.FirstOrDefaultAsync(s => s.ManagerId == managerId && s.SubordinateId == subordinateId);

            bool isRelated = hierarchy != null;

            return isRelated;
        }

        public static async Task<bool> HasManager(int subordinateId)
        {
            var db = new DeviseHrContext();

            var hierarchy = await db.Hierarchies.FirstOrDefaultAsync(s => s.SubordinateId == subordinateId);

            bool hasManager = hierarchy != null;

            return hasManager;
        }

        public static async Task<bool> HasSubordinate(int managetId)
        {
            var db = new DeviseHrContext();

            var hierarchy = await db.Hierarchies.FirstOrDefaultAsync(s => s.ManagerId == managetId);

            bool hasManager = hierarchy != null;

            return hasManager;
        }

        public static async Task<List<Hierarchy>> GetManagers(int subordinateId)
        {
            var db = new DeviseHrContext();

            var hierarchies = await db.Hierarchies.Where(s => s.SubordinateId == subordinateId).ToListAsync();

            return hierarchies;
        }

        public static async Task<List<Hierarchy>> GetSubordinate(int managetId)
        {
            var db = new DeviseHrContext();

            var hierarchies = await db.Hierarchies.Where(s => s.ManagerId == managetId).ToListAsync();

            return hierarchies;
        }
    }
}
