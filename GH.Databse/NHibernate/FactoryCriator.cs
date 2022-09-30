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
                    // This will set the command_timeout property on factory-level
                    cfg.SetProperty(NHibernate.Cfg.Environment.CommandTimeout, "180");
                    // This will set the command_timeout property on system-level
#pragma warning disable CS0618 // Тип или член устарел
                    NHibernate.Cfg.Environment.Properties.Add(NHibernate.Cfg.Environment.CommandTimeout, "180");
#pragma warning restore CS0618 // Тип или член устарел

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
