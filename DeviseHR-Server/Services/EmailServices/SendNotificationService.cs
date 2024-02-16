namespace DeviseHR_Server.Services.EmailServices
{
    public class SendNotificationService
    {
        public static async Task SendUserRegistration(string recipient, string firstName, string lastName, string code)
        {
            try
            {
                // Simulate an async delay of 2 seconds
                //  await Task.Delay(1000);

                Console.WriteLine(recipient);
                Console.WriteLine(firstName);
                Console.WriteLine(lastName);
                Console.WriteLine(code);
                Console.WriteLine("Fake email sent. Connect to SMTP Server for production.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
