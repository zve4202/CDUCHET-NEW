using GH.Database;
using GH.XlShablon;
using GH.XlShablon.Workers;
using System.Data;
using System.Threading;
using Tester.forms;

namespace Tester.Database
{
    public class ExcelVsDbWorker : Worker
    {
        IFactoryCriator factory;
        INHRepository repository;
        private object locker;
        const string sql = "select id, scan_type, prix_qty, stock_qty, orea_qty " +
            "from check_stock_orea_by_barcorde(:scan_type, :barcode, :st_id, :client_id)";


        public ExcelVsDbWorker(DataProcessor dataProcessor, CancellationToken cancellationToken, DataRow[] excelRows) : base(dataProcessor, cancellationToken, excelRows)
        {
            DbName = "CHECK_STOCK_OREA";
            string dbName = string.Format("{0}-{1}", DbName, index);
            factory = NHHelper.GetFactoryCriator(dbName);
            if (factory == null)
                factory = new FactoryCriatorTester(dbName);
            repository = new NHRepository<TestResult>(dbName);
        }

        protected override void ProcessRow(DataRow excelRow)
        {
            TestParams testParams = ExcelDbProcSetting.Setting.GetTestParams(dataProcessor.GetKeyValue(excelRow).ToString());
            object result = null;
            lock (locker)
            {
                result = repository.SelectFormProcedure(testParams, sql);
            }
            dataProcessor.ProcessRow(excelRow, result);
        }

    }
}
