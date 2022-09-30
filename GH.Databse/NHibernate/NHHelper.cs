using GH.Utils;
using NHibernate;
using System;
using System.Collections.Generic;

using static GH.Windows.DlgHelper;

namespace GH.Database
{
    public static class NHHelper
    {
        internal static IFactoryCriator BaseCriator;
        private static IDictionary<string, IFactoryCriator> _criators = new Dictionary<string, IFactoryCriator>();

        private static ISessionFactory _baseFactory;
        private static IDictionary<string, ISessionFactory> _factoryes = new Dictionary<string, ISessionFactory>();

        internal static void SetBaseFactoryCriator(IFactoryCriator factory)
        {
            BaseCriator = factory;
            _criators.Remove(factory.DbName);
        }

        public static void SetFactoryCriator(IFactoryCriator value)
        {
            if (_criators.Count == 0 && BaseCriator == null)
                SetBaseFactoryCriator(value);
            else
                _criators[value.DbName] = value;
        }


        private static ISessionFactory BaseSessionFactory
        {
            get
            {
                if (_baseFactory == null)
                    _baseFactory = BaseCriator.GetSessionFactory();

                return _baseFactory;
            }
        }

        public static ISession OpenMainSession()
        {
            ISession session = BaseSessionFactory.OpenSession();
            return session;
        }

        public static ISession OpenSession(string name)
        {
            try
            {

                ISession session = null;
                if (BaseCriator.DbName == name)
                    session = BaseSessionFactory.OpenSession();
                else
                    session = _factoryes[name].OpenSession();
                return session;
            }
            catch (Exception ex)
            {
                Logger.Error(nameof(OpenSession), ex);
                return null;
            }
        }

        public static bool Connect()
        {
            try
            {
                if (BaseSessionFactory != null)
                {
                    using (ISession session = OpenMainSession())
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                DlgError(ex.ToString());
            }
            return false;
        }

    }
}
