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
            EnglishDescription = "Logs you into the utility bot against the allowed users list (for partyline access)";
        }
        public override void Process(WCell.Util.Commands.CmdTrigger<IrcCmdArgs> trigger)
        {
            var sessionFactory = DBHandler.DBHandler.CreateSessionFactory();
            using(var session = sessionFactory.OpenSession())
            {
                using(var transaction = session.BeginTransaction())
                {
                    var acc = new Account {Level = AccountLevel.SuperAdmin, PartylineUsername = "jaddie", QUsername = "jaddie" };
                    session.SaveOrUpdate(acc);
                    transaction.Commit();
                }
                using(session.BeginTransaction())
                {
                    var accounts = session.CreateCriteria(typeof (Account)).List();
                    foreach (var account in accounts)
                    {
                        Console.WriteLine(account);
                        var acc = (Account) account;
                        trigger.Reply("Id: " + acc.Id + " AccountLevel: " + acc.Level + " PartylineUsername: " + acc.PartylineUsername + " QUsername: " + acc.QUsername);
                    }
                }
            }
        }
    }
}