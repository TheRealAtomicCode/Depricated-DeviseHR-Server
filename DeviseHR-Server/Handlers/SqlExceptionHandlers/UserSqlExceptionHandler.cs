using Microsoft.EntityFrameworkCore;

namespace DeviseHR_Server.Handlers.SqlExceptionHandlers
{
    public class UserSqlExceptionHandler
    {
        public static void InsertUserSqlExceptionHandler(DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.StartsWith("23505"))
            {
                throw new Exception("Email already exists");
            }
            else
            {
                throw new Exception("Invalid data provided");
            }
            throw ex;
        }
    }
}
