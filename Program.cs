using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WCellUtilityBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0 && args.Any(arg => arg.StartsWith("con") | arg.StartsWith("console") | arg == "c"))
            {
                UtilityBotService.Run(true);
                do {
                        Thread.Sleep(10);
                   }
                while (true) ;
            }
            ServiceBase[] servicesToRun = new ServiceBase[] { new UtilityBotService() };
            ServiceBase.Run(servicesToRun);
        }
    }
}
