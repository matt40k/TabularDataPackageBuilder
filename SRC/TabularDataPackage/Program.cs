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
            logger.Log(LogLevel.Trace, "Main");
            if (args.Length == 0)
            {
                // Run User interface
                logger.Log(LogLevel.Trace, "Running User Interface");
                Application ui = new Application(); 
                ui.Run(new UserInterface()); 
            }
            else
            {
                // Run in console mode
                logger.Log(LogLevel.Trace, "Running Console mode");
                foreach (string arg in args)
                {
                    logger.Log(LogLevel.Trace, "Arg: " + arg); 
                }
            }

        }
    }
}
