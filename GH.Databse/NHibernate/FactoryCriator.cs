using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GH.Utils;
using NHibernate;
using System;

namespace GH.Database
{
    public class FactoryCriator<TFactoryCriator, TEntity> : IFactoryCriator
        where TFactoryCriator : IFactoryCriator
        where TEntity : BaseEntity
    {
        private string _dbName;
        public string DbName => _dbName;

        public FactoryCriator(string dbName)
        {
            _dbName = dbName;
            NHHelper.SetFactoryCriator(this);
        }


        protected virtual IPersistenceConfigurer GetConfig()
        {
            throw new NotImplementedException(nameof(GetConfig));
        }

        public ISessionFactory GetSessionFactory()
        {
            return Fluently.Configure()
                .Database(GetConfig())
                .ExposeConfiguration(cfg =>
                {
                    cfg.SetProperty(NHibernate.Cfg.Environment.CommandTimeout, "180");

                })
                .Mappings(cfg => cfg.FluentMappings.AddFromAssemblyOf<TEntity>())
                .BuildSessionFactory();
        }

        public virtual string GetConnectionString()
        {
            try
            {
                throw new NotImplementedException(nameof(GetConnectionString));
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return string.Empty;
            }
        }
    }
}
