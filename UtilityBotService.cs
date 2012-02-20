using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WCellUtilityBot
{
    public partial class UtilityBotService : ServiceBase
    {
        public UtilityBotService()
        {
            InitializeComponent();
        }

        public static void Run(bool consoleMode)
        {
            TaskEx.Run(() => { IrcConnection.Irc.BeginConnect(Properties.Settings.Default.IrcServer, Properties.Settings.Default.IrcPort);
                               IrcConnection.Irc.ConsoleMode = consoleMode;
            });
            TaskEx.Run(CommitListener.StartListener);
        }

        protected override void OnStart(string[] args)
        {
            Run(false);
        }

        protected override void OnStop()
        {
            IrcConnection.Irc.Client.Disconnect();
            Environment.Exit(0);
        }
    }
}
