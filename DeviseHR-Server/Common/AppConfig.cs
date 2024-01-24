namespace DeviseHR_Server.Common
{
    public static class AppConfig
    {
        public static IConfiguration ENV { get; } = new ConfigurationBuilder()
                .AddJsonFile("Env.json", optional: true, reloadOnChange: true)
                .Build();
    }
}

