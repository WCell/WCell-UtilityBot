using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squishy.Irc;

namespace WCellUtilityBot
{
    class IrcConnection : IrcClient
    {
        public static readonly IrcConnection Irc = new IrcConnection
        {
            UserName = Properties.Settings.Default.IrcUser,
            Nicks = new[] { Properties.Settings.Default.IrcNick },
            ServerPassword = Properties.Settings.Default.IrcPassword,
        };

        public bool ConsoleMode;
        protected override void Perform()
        {
            if(ConsoleMode)
            protHandler.PacketReceived += Console.WriteLine;
        }
        protected override void OnBeforeSend(string text)
        {
            if (ConsoleMode)
            Console.WriteLine(text);
        }
        public override bool MayTriggerCommand(WCell.Util.Commands.CmdTrigger<Squishy.Irc.Commands.IrcCmdArgs> trigger, Squishy.Irc.Commands.IrcCommand cmd)
        {
            return trigger.Args.User.UserLevel >= cmd.RequiredAccountLevel;
        }
    }
}
