using System.Diagnostics;

namespace DMarket_Bot
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Console.WriteLine("Selam, Moruq!");
            Debug.WriteLine("Selam, Moruq!");

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());


        }
    }
}