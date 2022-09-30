using GH.Utils;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GH.Database
{
    public enum SqlTypes
    {
        SelectSql,
        InsertSql,
        UpdateSql,
        SaveOrUpdateSql,
        CloseDocSql,
        DeleteSql,
        RefreshSql,
        ExecuteSql
    }

    public class NHRepositorySetting
    {
        static int _pageSize = 10000;
        public static int PageSize { get => _pageSize; set => _pageSize = value; }

        private int _pageNumber = 1;
        public int PageNumber { get => _pageNumber; set => _pageNumber = value; }
    }

    public class NHRepository<T> : NHRepositorySetting, INHRepository where T : BaseEntity
    {
        public Type ConcreteType => typeof(T);

        private Dictionary<SqlTypes, string> _queries = new Dictionary<SqlTypes, string>();

        private readonly string DbName;
        public NHRepository(string dbName)
        {
            DbName = dbName;
        }
        public void SetQuery(SqlTypes type, string query)
        {
            _queries.Add(type, query);
        }

        private ISession OpenSession()
        {
            return NHHelper.OpenSession(DbName);
        }

        public Func<SqlTypes, BaseEntity, string> GetSQL { get; set; } = null;

        private string GetSql(SqlTypes sqlType, T entity)
        {
            string query = null;
            _queries.TryGetValue(sqlType, out query);
            if (query == null && GetSQL != null)
            {
                query = GetSQL.Invoke(sqlType, entity);
                _queries.Add(sqlType, query);
            }
            return query;
        }

        #region DELETE
        public void Delete(object entity)
        {
            Delete((T)entity);

        }

        private void Delete(T entity)
        {
            using (ISession session = OpenSession())
            {
                if (session != null)
                {
                    string sql = GetSql(SqlTypes.DeleteSql, entity);
                    try
                    {
                        ITransaction transaction = session.BeginTransaction();
                        if (sql == null)
                            session.Delete(entity);
                        else
                            session.CreateSQLQuery(sql).ExecuteUpdate();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, Delete", ex);
                    }
                }
            }

        }

        public void DeleteAll(ICollection<T> entitys)
        {
            using (ISession session = OpenSession())
            {
                try
                {
                    ITransaction transaction = session.BeginTransaction();
                    foreach (T entity in entitys)
                    {
                        string sql = GetSql(SqlTypes.DeleteSql, entity);

                        if (sql == null)
                            session.Delete(entity);
                        else
                            session.CreateSQLQuery(sql).ExecuteUpdate();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Logger.Error("NHRepository, DeleteAll", ex);
                }
            }

        }

        public void ExequteQuery(string[] sql)
        {
            using (ISession session = OpenSession())
            {
                try
                {
                    ITransaction transaction = session.BeginTransaction();
                    foreach (string item in sql)
                        session.CreateSQLQuery(item).ExecuteUpdate();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Logger.Error("NHRepository, ExequteQuery", ex);
                }
            }
        }

        public BaseEntity Get(object id)
        {
            BaseEntity bindable = null;

            using (ISession session = OpenSession())
            {
                try
                {
                    bindable = session.Get<BaseEntity>(id);
                }
                catch (Exception ex)
                {
                    Logger.Error("NHRepository, Get", ex);
                }
            }

            return bindable;
        }

        #endregion

        #region INSERT

        public void Save(object entity)
        {
            Save((T)entity);
        }

        private void Save(T entity)
        {
            using (ISession session = OpenSession())
            {
                if (session != null)
                {
                    string sql = GetSql(SqlTypes.InsertSql, entity);
                    try
                    {
                        ITransaction transaction = session.BeginTransaction();
                        object obj = null;
                        if (sql == null)
                        {
                            session.Save(entity);
                        }
                        else
                            obj = session.CreateSQLQuery(sql).AddEntity(typeof(T)).UniqueResult<T>();
                        transaction.Commit();

                        if (obj == null)
                            Refresh(entity);
                        else
                            entity.Assigne(obj);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, Save", ex);
                    }
                }
            }
        }
        #endregion

        #region UPDATE
        public void SaveOrUpdate(object entity)
        {
            Update((T)entity);
        }

        private void Update(T entity)
        {
            string sql = GetSql(SqlTypes.UpdateSql, entity);
            using (ISession session = OpenSession())
            {
                if (session != null)
                {
                    try
                    {
                        ITransaction transaction = session.BeginTransaction();
                        if (sql == null)
                            session.Update(entity);
                        else
                            session.CreateSQLQuery(sql).ExecuteUpdate();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, Update", ex);
                    }
                }
            }
        }
        #endregion

        #region INSERT OR UPDATE
        public void SaveOrUpdate(T entity)
        {
            string sql = GetSql(SqlTypes.SaveOrUpdateSql, entity);
            using (ISession session = OpenSession())
            {
                if (session != null)
                {
                    try
                    {
                        ITransaction transaction = session.BeginTransaction();
                        if (sql == null)
                            session.SaveOrUpdate(entity);
                        else
                        {
                            BaseEntity query = session.CreateSQLQuery(sql).AddEntity(typeof(BaseEntity)).UniqueResult<BaseEntity>();
                            (entity as BaseEntity).Id = query.Id;
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, SaveOrUpdate", ex);
                    }
                }
            }
        }

        #endregion

        #region CLOSE OPEN DOCUMENT
        public void CloseOpenDoc(object entity)
        {
            CloseOpenDoc((T)entity);
        }

        private void CloseOpenDoc(T entity)
        {
            using (ISession session = OpenSession())
            {
                if (session != null)
                {
                    string sql = GetSql(SqlTypes.CloseDocSql, entity);
                    try
                    {
                        ITransaction transaction = session.BeginTransaction();
                        if (sql == null)
                            session.Update(entity);
                        else
                            session.CreateSQLQuery(sql).ExecuteUpdate();
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, CloseOpenDoc", ex);
                    }
                }
            }
        }
        #endregion

        #region UPDATE ALL

        public void UpdateAll(ICollection<T> entitys)
        {
            using (ISession session = OpenSession())
            {
                if (session != null)
                {
                    try
                    {
                        ITransaction transaction = session.BeginTransaction();
                        foreach (T entity in entitys)
                        {
                            string sql = GetSql(SqlTypes.UpdateSql, entity);

                            if (sql == null)
                                session.Update(entity);
                            else
                                session.CreateSQLQuery(sql).ExecuteUpdate();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, UpdateAll", ex);
                    }
                }
            }
        }
        #endregion

        #region REFRESH

        public void Refresh(object entity)
        {
            Refresh((T)entity);
        }

        private void Refresh(T entity)
        {
            string sql = GetSql(SqlTypes.RefreshSql, entity);

            using (ISession session = OpenSession())
            {
                if (session != null)
                {
                    try
                    {
                        if (sql == null)
                            session.Refresh(entity);
                        else
                            entity.Assigne(session.CreateSQLQuery(sql).AddEntity(typeof(T)).UniqueResult<T>());
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, Refresh", ex);
                    }
                }
            }
        }
        #endregion

        #region REPLICATE
        public void Replicate(T entity)
        {

            using (ISession session = OpenSession())
            {
                if (session != null)
                {
                    try
                    {
                        ITransaction transaction = session.BeginTransaction();
                        session.Replicate(entity, ReplicationMode.Overwrite);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, Replicate", ex);
                    }
                }
            }
        }
        #endregion

        #region SELECT
        public Func<Dictionary<string, bool>> GetSorting { get; set; }

        private Dictionary<string, bool> Sorting()
        {
            if (GetSorting != null)
                return GetSorting.Invoke();
            return null;
        }

        public Func<Dictionary<string, object>> GetParams { get; set; } = null;




        private Dictionary<string, object> Params()
        {
            if (GetParams != null)
                return GetParams.Invoke();
            return null;
        }

        public IList<T> AsTypeList()
        {
            var lst = InnerSelectAll();
            return lst;
        }

        private IList<T> InnerSelectAll()
        {
            string sql = GetSql(SqlTypes.SelectSql, Activator.CreateInstance<T>());
            List<T> lst = null;
            Dictionary<string, bool> sort = Sorting();
            Dictionary<string, object> pars = Params();
            try
            {
                using (ISession session = OpenSession())
                {
                    if (session != null)
                    {
                        try
                        {
                            if (sql == null)
                            {
                                ICriteria data = session.CreateCriteria<T>();
                                if (pars != null)
                                {
                                    foreach (KeyValuePair<string, object> item in pars)
                                        if (item.Value == null)
                                            data.Add(Expression.IsNull(item.Key));
                                        else
                                            data.Add(Expression.Eq(item.Key, item.Value));
                                }


                                if (sort != null)
                                    foreach (KeyValuePair<string, bool> item in sort)
                                        data.AddOrder(new Order(item.Key, item.Value));


                                lst = data.List<T>().ToList();
                            }
                            else
                            {
                                ISQLQuery query = session.CreateSQLQuery(sql).AddEntity(typeof(T));

                                if (pars != null)
                                {
                                    foreach (KeyValuePair<string, object> item in pars)
                                        query.SetParameter(item.Key, item.Value);
                                }

                                lst = query.List<T>().ToList();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("NHRepository, InnerSelectAll", ex);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Fatal("NHRepository InnerSelectAll", ex);
            }
            return lst ?? new List<T>().ToList();
        }

        private IList<T> InnerSelectAllAsync()
        {
            IList<T> lst = InnerSelectAll();

            return lst ?? new List<T>();
        }

        public IList SelectAll()
        {
            IList<T> lst = InnerSelectAllAsync();
            return lst.ToList();
        }

        BaseEntity Select()
        {
            BaseEntity entity = null;
            try
            {
                using (ISession session = OpenSession())
                {
                    string sql = GetSql(SqlTypes.SelectSql, null);
                    Dictionary<string, object> pars = Params();
                    try
                    {
                        if (sql == null)
                        {
                            ICriteria data = session.CreateCriteria<T>();
                            if (pars != null)
                            {
                                foreach (KeyValuePair<string, object> item in pars)
                                    if (item.Value == null)
                                        data.Add(Expression.IsNull(item.Key));
                                    else
                                        data.Add(Expression.Eq(item.Key, item.Value));
                            }

                            entity = data.List<T>().FirstOrDefault();
                        }
                        else
                        {
                            entity = session.CreateSQLQuery(sql).AddEntity(typeof(T)).List<T>().FirstOrDefault();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository, Select()", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal("NHRepository Select()", ex);
            }
            return entity;
        }

        public BaseEntity SelectOne()
        {
            return Select();
        }

        public object SelectFormProcedure(object entity, string sql)
        {
            object res = null;
            try
            {
                using (ISession session = OpenSession())
                {
                    try
                    {
                        ITransaction transaction = session.BeginTransaction();
                        IQuery query = session.CreateSQLQuery(sql).AddEntity(typeof(T));
                        SetParams(entity, query);

                        res = query.UniqueResult<T>();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("NHRepository", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal("NHRepository", ex);
            }
            return res;
        }

        private static void SetParams(object entity, IQuery query)
        {
            if (query.NamedParameters.Length == 0)
                return;

            foreach (string par in query.NamedParameters)
            {
                PropertyInfo info = entity.GetFullAccessOnlyProperties().First(p => p.Name == par);
                object value = info.GetValue(entity);

                if (value == null)
                {
                    Type type = GetPureType(info);

                    if (type == typeof(int))
                        query.SetParameter(par, value, NHibernateUtil.Int32);
                    else
                    if (type == typeof(decimal))
                        query.SetParameter(par, value, NHibernateUtil.Decimal);
                    else
                    if (type == typeof(double))
                        query.SetParameter(par, value, NHibernateUtil.Double);
                    else
                    if (type == typeof(DateTime))
                        query.SetParameter(par, value, NHibernateUtil.DateTime);
                    else
                    if (type == typeof(string))
                        query.SetParameter(par, value, NHibernateUtil.String);
                    else
                    if (type == typeof(bool))
                        query.SetParameter(par, value, NHibernateUtil.Boolean);
                    else
                        query.SetParameter(par, value);
                }
                else
                    query.SetParameter(par, value);
            }
        }

        private static Type GetPureType(PropertyInfo info)
        {
            if (info.PropertyType.IsGenericType)
            {
                Type[] types = info.PropertyType.GetGenericArguments();
                if (types.Length == 1)
                    return types[0];
            }
            return info.PropertyType;
        }

        public Dictionary<int, string> KeyIntLookupList()
        {
            return InnerSelectAll().ToDictionary(x => x.Id, x => x.Name);
        }

        public Dictionary<BaseEntity, string> KeyEntityLookupList()
        {
            return InnerSelectAll().ToDictionary(x => (BaseEntity)x, x => x.Name);
        }

        #endregion
    }
}
