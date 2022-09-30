using GH.Database;
using GH.Utils;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Tester.Database;
using Tester.forms;
using static GH.XlShablon.FieldParam;

namespace GH.XlShablon
{
    public class ExcelVsDbProcessor : DataProcessor
    {
        IFactoryCriator factory;
        INHRepository clients;


        const string skladQuery = "select id, prix_qty, stock_qty from check_sklad_stock_by_barcorde(:st_id, :barcode)";
        const string oreaQuery = "select id, orea_qty from check_orea_stock_by_barcorde(:client_id, :st_id, :barcode)";
        private Dictionary<string, INHRepository> requests = new Dictionary<string, INHRepository>();

        private ExcelDbProcSetting _procSetting;
        internal ExcelDbProcSetting ProcSetting
        {
            get => _procSetting;
            set
            {
                _procSetting = value;
                if (_procSetting != null)
                {
                    _procSetting.Clients = clients.KeyIntLookupList();
                }
            }
        }

        public ExcelVsDbProcessor(Control control, IFactoryCriator factory) : base(control)
        {
            this.factory = factory;
            clients = new NHRepository<Client>(factory.DbName);
            clients.GetSorting += GetClientSorting;
        }



        public override void CreateOutsourceMap(FieldsMap dataMap)
        {
            foreach (KeyValuePair<string, INHRepository> item in requests)
            {
                foreach (PropertyInfo prop in item.Value.ConcreteType.GetNestedProperties())
                {
                    FieldParam field = new FieldParam(
                        Shablon,
                        prop.GetCustomAttributes<UpdatablePropertyAttribute>().First().Caption,
                        prop.Name,
                        prop.PropertyType, 10, true)
                    {
                        ParamFunc = ParamFunctionType.OutSourceData
                    };
                    dataMap.Add(field);
                }
            }
        }

        protected override void BeginProcess()
        {
            base.BeginProcess();
            requests.Clear();
            switch (ProcSetting.TestType)
            {
                case 0:
                    requests.Add(skladQuery, new NHRepository<ResultSklad>(factory.DbName));
                    break;
                case 1:
                    requests.Add(oreaQuery, new NHRepository<ResultOrea>(factory.DbName));
                    break;
                case 2:
                    requests.Add(skladQuery, new NHRepository<ResultSklad>(factory.DbName));
                    requests.Add(oreaQuery, new NHRepository<ResultOrea>(factory.DbName));
                    break;
                default:
                    break;
            }
        }


        private Dictionary<string, bool> GetClientSorting()
        {
            return new Dictionary<string, bool>
            {
                {"Name", true}
            };
        }

        protected override bool AddCalculateFields(int index)
        {
            return false;
        }

        protected override void SetOutsourceFieds(DataRow row)
        {
            Params query_params = new Params(ProcSetting.client_id, ProcSetting.stock_id, row.Field<string>(0));
            foreach (KeyValuePair<string, INHRepository> item in requests)
            {
                object result = item.Value.SelectFormProcedure(query_params, item.Key);
                foreach (PropertyInfo prop in result.GetNestedProperties())
                {
                    row.SetField<object>(prop.Name, int.Parse(prop.GetValue(result).ToString()));
                }
            }
        }

    }
}
