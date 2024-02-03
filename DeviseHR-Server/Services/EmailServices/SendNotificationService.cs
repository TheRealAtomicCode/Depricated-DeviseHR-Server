
namespace DeviseHR_Server.Services.EmailServices
{
    public class SendNotificationService
    {
        public static void SendUserRegistration(string recipient, string firstName, string lastName, string code)
        {
            try
            {
                Console.WriteLine(recipient);
                Console.WriteLine(firstName);
                Console.WriteLine(lastName);
                Console.WriteLine(code);
                Console.WriteLine("fake email sent, Connect to SMTP Server for production");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}


