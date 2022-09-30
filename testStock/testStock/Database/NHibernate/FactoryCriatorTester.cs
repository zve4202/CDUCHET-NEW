using FluentNHibernate.Cfg.Db;
using GH.Configs;
using GH.Database;
using GH.Utils;
using System;

namespace Tester.Database
{
    public class FactoryCriatorTester : FactoryCriator<FactoryCriatorTester, Client>
    {
        public FactoryCriatorTester() : base("CDUCHET")
        {
        }

        protected override IPersistenceConfigurer GetConfig()
        {
            return new FirebirdConfiguration().ConnectionString(GetConnectionString());
        }


        public override string GetConnectionString()
        {
            try
            {
                return IniHelper.Cfg<CfgCdStore>().ConnectionString();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return string.Empty;
            }
        }
    }
}
