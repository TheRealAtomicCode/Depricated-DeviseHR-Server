namespace DeviseHR_Server.Helpers
{
    public class StringGeneration
    {
        public static string GenerateSixDigitString()
        {
            Random random = new Random();
            int code = random.Next(100000000, 1000000000);
            return code.ToString();
        }
    }
}
