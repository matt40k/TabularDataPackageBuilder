using System;
using System.Windows;

namespace TabularDataPackage
{
    class Program
    {
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
