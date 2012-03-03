using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squishy.Irc.Commands;
using WCellUtilityBot.Entities;

namespace WCellUtilityBot
{
    class CreateAccountCommand : IrcCommand
    {
        protected override void Initialize()
        {
            Init("ca","createaccount");
            EnglishParamInfo = "partylineUser qUsername";
        }
        public override void Process(WCell.Util.Commands.CmdTrigger<IrcCmdArgs> trigger)
        {
            using (var sessionFactory = DBHandler.DBHandler.CreateSessionFactory())
                using (var session = sessionFactory.OpenSession())
                    using (var transaction = session.BeginTransaction())
                    {
                        var partylineUser = trigger.Text.NextWord();
                        var qUsername = trigger.Text.NextWord();
                        var acc = new Account { Level = AccountLevel.Guest, PartylineUsername = partylineUser, QUsername = qUsername };
                        session.SaveOrUpdate(acc);
                        transaction.Commit();
                        trigger.Reply("Created account with partylineuser: " + partylineUser + " and Qusername: " + qUsername);
                    }
        }
    }
}
