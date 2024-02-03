namespace DeviseHR_Server.Helpers
{
    public class StringGeneration
    {
        public static string GenerateSixDigitString()
        {
            Random random = new Random();
            int code = random.Next(1000000, 10000000);
            return code.ToString();
        }
    }
}
