using GH.Database;
using GH.XlShablon.Workers;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Tester.Database;
using Tester.forms;

namespace GH.XlShablon
{
    public class ExcelVsDbProcessor : DataProcessor
    {
        IFactoryCriator factory;
        INHRepository clients;


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

        public ExcelVsDbProcessor(Control control) : base(control)
        {
            factory = new FactoryCriatorTester();
            clients = new NHRepository<Client>(factory.DbName);
            clients.GetSorting += GetClientSorting;
        }

        protected override Worker GetWorker(DataRow[] excelRows)
        {
            return new Worker(this, Shablon.CancellationToken, excelRows);
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

    }
}
