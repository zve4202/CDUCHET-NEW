using NHibernate;

namespace GH.Database
{
    public interface IFactoryCriator
    {
        string DbName { get; }
        ISessionFactory GetSessionFactory();
    }
}
