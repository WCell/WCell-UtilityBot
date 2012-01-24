using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squishy.Irc.Commands;

namespace WCellUtilityBot.Entities
{
    public class Account
    {
        public virtual int Id { get; private set; }
        public virtual string PartylineUsername { get; set; }
        public virtual string QUsername { get; set; }
        public virtual AccountLevel Level { get; set; }
    }
}
