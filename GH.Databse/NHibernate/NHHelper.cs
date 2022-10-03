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

        private static IDictionary<string, ISessionFactory> _factories = new Dictionary<string, ISessionFactory>();

        internal static void SetBaseFactoryCriator(IFactoryCriator factory)
        {
            BaseCriator = factory;
            _criators.Remove(factory.DbName);
        }

        public static IFactoryCriator GetFactoryCriator(string dbName)
        {
            if (BaseCriator.DbName == dbName)
                return BaseCriator;
            IFactoryCriator criator;
            _criators.TryGetValue(dbName, out criator);
            return criator;
        }

        public static void SetFactoryCriator(IFactoryCriator value)
        {
            if (_criators.Count == 0 && BaseCriator == null)
                SetBaseFactoryCriator(value);
            else
                _criators.Add(value.DbName, value);
        }


        private static ISessionFactory _baseFactory;
        private static ISessionFactory BaseSessionFactory
        {
            get
            {
                if (_baseFactory == null)
                    _baseFactory = BaseCriator.GetSessionFactory();
                return _baseFactory;
            }
        }

        public static ISessionFactory SessionFactory(string dbName)
        {
            ISessionFactory factory = null;
            _factories.TryGetValue(dbName, out factory);
            if (factory == null)
            {
                factory = _criators[dbName].GetSessionFactory();
                _factories.Add(dbName, factory);
            }
            return factory;

        }

        //private static ISessionFactory SessionFactory
        //{
        //    get
        //    {
        //        if (_baseFactory == null)
        //            _baseFactory = BaseCriator.GetSessionFactory();

        //        return _baseFactory;
        //    }
        //}


        public static ISession OpenMainSession()
        {
            ISession session = BaseSessionFactory.OpenSession();
            return session;
        }

        private static object session_locker = new object();

        private static bool IsBase(string name)
        {
            return BaseCriator.DbName == name;
        }

        public static ISession OpenSession(string name)
        {
            try
            {
                ISession session = null;
                lock (session_locker)
                {
                    if (IsBase(name))
                        session = BaseSessionFactory.OpenSession();
                    else
                        session = SessionFactory(name).OpenSession();
                }
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

        public static void ClearCriators()
        {
            _factories.Clear();
            _criators.Clear();
        }


    }
}
