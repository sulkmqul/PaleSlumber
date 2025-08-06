namespace PaleSlumber
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (SingleInstanceApplication uq = new SingleInstanceApplication(PaleConst.ApplicationName))
            {
                if (uq.DuplicateFlag == true)
                {
                    uq.ActivateAnother();
                    return;
                }

                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
        }
    }
}