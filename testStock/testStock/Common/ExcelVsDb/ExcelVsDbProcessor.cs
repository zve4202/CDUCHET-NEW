using GH.Database;
using GH.XlShablon.Workers;
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

        public ExcelVsDbProcessor(Control control) : base(control)
        {
            factory = new FactoryCriatorTester();
            clients = new NHRepository<Client>(factory.DbName);
            clients.GetSQL += GetClientSql;
        }

        protected override ProcessScanType ProcessScanType => ProcessScanType.AsIs;


        private ExcelDbProcSetting _procSetting;
        internal ExcelDbProcSetting ProcSetting
        {
            get => _procSetting;
            set
            {
                _procSetting = value;
                if (_procSetting != null)
                {
                    _procSetting.IClients = clients;
                }
            }
        }


        public override void CreateOutsourceMap(FieldsMap dataMap)
        {
            foreach (PropertyInfo prop in ProcSetting.GetScanTypeProperties())
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

        private string GetClientSql(SqlTypes sqlTypes, BaseEntity entity)
        {
            switch (sqlTypes)
            {
                case SqlTypes.SelectSql:
                    return "select distinct " +
                        "c.client_id, " +
                        "c.client_name " +
                        "from clients c " +
                        "inner join orea_clients oc on (c.client_id = oc.client_id) " +
                        "order by c.client_name";
                default:
                    return null;
            }
        }

        protected override Worker GetWorker(DataRow[] excelRows)
        {
            return new ExcelVsDbWorker(this, Shablon.CancellationToken, excelRows);
        }


        protected override bool AddCalculateFields()
        {
            return false;
        }

    }
}
