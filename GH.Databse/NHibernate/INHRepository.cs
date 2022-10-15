using System;
using System.Collections;
using System.Collections.Generic;

namespace GH.Database
{
    public interface INHRepository
    {
        Type ConcreteType { get; }
        Func<SqlTypes, BaseEntity, string> GetSQL { get; set; }
        Func<Dictionary<string, bool>> GetSorting { get; set; }
        Func<Dictionary<string, object>> GetParams { get; set; }

        Dictionary<int, string> KeyIntLookupList();
        Dictionary<BaseEntity, string> KeyEntityLookupList();

        void Delete(object entity);
        BaseEntity Get(object id);
        void Save(object entity);
        void SaveOrUpdate(object entity);
        void Refresh(object entity);
        void CloseOpenDoc(object entity);
        IList SelectAll();
        BaseEntity SelectOne();
        object SelectFormProcedure(object entity, string sql, bool withCommite = true);
        void ExequteQuery(string[] sql);
    }
}
