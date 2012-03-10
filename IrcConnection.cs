using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squishy.Irc;
using WCell.Util;
using WCellUtilityBot.ChannelManagment;

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
        protected override void OnConnecting()
        {
            base.OnConnecting();
            if (ConsoleMode)
                protHandler.PacketReceived += Console.WriteLine;
        }
        protected override void Perform()
        {
            base.Perform();
            var qUser = Properties.Settings.Default.QUser;
            var qPass = Properties.Settings.Default.QPass;
            if(!string.IsNullOrWhiteSpace(qUser) && string.IsNullOrWhiteSpace(qPass))
            {
                if(ConsoleMode)
                    Console.WriteLine("Authing with Q.." + qUser);
                CommandHandler.Msg("Q@Cserve.quakenet.org","auth " + qUser + " " + qPass);
            }
        }
        protected override void OnBeforeSend(string text)
        {
            base.OnBeforeSend(text);
            if (ConsoleMode)
            Console.WriteLine(text);
        }
        protected override void OnCommandFail(WCell.Util.Commands.CmdTrigger<Squishy.Irc.Commands.IrcCmdArgs> trigger, Exception ex)
        {
            CommandHandler.Msg(Properties.Settings.Default.ErrorChannel, "Exception Occured: " + ex.InnerException.Message);
            foreach (string text in ex.GetAllMessages())
                CommandHandler.Msg(Properties.Settings.Default.ErrorChannel, text);
        }
        public override bool MayTriggerCommand(WCell.Util.Commands.CmdTrigger<Squishy.Irc.Commands.IrcCmdArgs> trigger, Squishy.Irc.Commands.IrcCommand cmd)
        {
            return trigger.Args.User.UserLevel >= cmd.RequiredAccountLevel;
        }
        protected override void OnJoin(IrcUser user, IrcChannel chan)
        {
            base.OnJoin(user, chan);
            new AutoVoiceUser(user, chan);
        }
    }
}
