using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squishy.Irc.Commands;
using WCellUtilityBot.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
namespace WCellUtilityBot
{
    class LoginCommand : IrcCommand
    {
        protected override void Initialize()
        {
            Init("login","l");
            EnglishDescription = "Logs you into the utility bot";
        }
        public override void Process(WCell.Util.Commands.CmdTrigger<IrcCmdArgs> trigger)
        {
            //TODO: Write code to login to the bot
        }
    }
}