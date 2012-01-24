using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using WCellUtilityBot.Entities;

namespace WCellUtilityBot.Mappings
{
    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Id(x => x.Id);
            Map(x => x.PartylineUsername);
            Map(x => x.QUsername);
            Map(x => x.Level);
        }
    }
}
