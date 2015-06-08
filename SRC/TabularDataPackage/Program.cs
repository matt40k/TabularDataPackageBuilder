using System;
using System.Windows;
using NLog;

namespace TabularDataPackage
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                // Run User interface
                Application ui = new Application(); 
                ui.Run(new UserInterface()); 

            }
            else
            {
                // Run in console mode
            }

        }
    }
}
