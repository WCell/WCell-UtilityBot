using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCell.Util.Commands;

namespace Squishy.Irc.Commands
{
    public enum AccountLevel
    {
        Guest = 1,
        User = 2,
        Staff = 3,
        SeniorAssistants = 4,
        Admin = 5,
        SuperAdmin = 6
    }

    public abstract class IrcCommand : Command<IrcCmdArgs>
    {
        public AccountLevel RequiredAccountLevel = AccountLevel.Guest;
    }
}
