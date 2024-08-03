namespace RapidMessageCast_Manager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Initialize the application and run the main form
            ApplicationConfiguration.Initialize();
            Application.Run(new RMCManager());
        }
    }
}