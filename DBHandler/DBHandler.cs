using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using WCellUtilityBot.Entities;

namespace WCellUtilityBot.DBHandler
{
    class DBHandler
    {
        public static ISessionFactory CreateSessionFactory()
        {
            var fluconf = Fluently.Configure();
            var dbconf = MySQLConfiguration.Standard.ConnectionString(builder =>
                                                                                    {
                                                                                        builder.Server(Properties.Settings.Default.MysqlHost);
                                                                                        builder.Username(Properties.Settings.Default.MysqlUser);
                                                                                        builder.Password(Properties.Settings.Default.MysqlPassword);
                                                                                        builder.Database(Properties.Settings.Default.MysqlDB);
                                                                                    });
            fluconf.Database(dbconf);
            fluconf.Mappings(m => m.FluentMappings.AddFromAssemblyOf<Account>());
            fluconf.ExposeConfiguration(configuration => new SchemaUpdate(configuration).Execute(false,true));
            return fluconf.BuildSessionFactory();
        }
    }
}
