using System;
using System.Windows;
using NLog;

namespace TabularDataPackage
{
    internal class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [STAThread]
        private static void Main(string[] args)
        {
            logger.Log(LogLevel.Trace, "Main");
            if (args.Length == 0)
            {
                // Run User interface
                logger.Log(LogLevel.Trace, "Running User Interface");
                var ui = new Application();
                ui.Run(new UserInterface());
            }
            else
            {
                // Run in console mode
                logger.Log(LogLevel.Trace, "Running Console mode");
                foreach (var arg in args)
                {
                    logger.Log(LogLevel.Trace, "Arg: " + arg);
                }
            }
        }
    }
}