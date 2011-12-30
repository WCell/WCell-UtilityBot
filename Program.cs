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
            if (args != null && args.Length > 0 && Regex.IsMatch("console|c|con",args.ToString()))
            {
                Thread.Sleep(10);
            }
            else
            {
                ServiceBase[] servicesToRun = new ServiceBase[] 
                                                  { 
                                                      new UtilityBotService() 
                                                  };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
