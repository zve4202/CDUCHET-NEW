using GH.Database;
using GH.XlShablon.Workers;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using Tester.Database;
using Tester.forms;

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
            foreach (PropertyInfo item in ProcSetting.GetScanTypeProperties())
            {
                dataMap.add

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


        protected override bool AddCalculateFields(int index)
        {
            return false;
        }

    }
}
