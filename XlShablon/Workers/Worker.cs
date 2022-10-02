using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GH.XlShablon.Workers
{
    public class Worker
    {
        public Worker(DataProcessor dataProcessor, CancellationToken cancellationToken, DataRow[] excelRows)
        {
            this.dataProcessor = dataProcessor;
            this.cancellationToken = cancellationToken;
            index = WorkersPull.CurrentWorker;
            ExcelRows = excelRows;
            WorkersPull.AddToPool(this);
        }

        public string DbName { get; set; }
        protected DataProcessor dataProcessor;
        protected readonly int index;
        internal readonly CancellationToken cancellationToken;
        private DataRow[] ExcelRows;
        private DataTable ResultData => dataProcessor.ResultData;

        public bool CanWork => !cancellationToken.IsCancellationRequested;

        internal async void Execute()
        {
            await Task.Factory.StartNew(() => DoWork());
        }

        private void DoWork()
        {
            foreach (DataRow excelRow in ExcelRows)
            {
                if (!CanWork)
                {
                    ExcelRows = null;
                    break;
                }

                ProcessRow(excelRow);
            }
            WorkersPull.RemoveFromPool(this);
        }

        protected virtual void ProcessRow(DataRow excelRow)
        {
            dataProcessor.ProcessRow(excelRow);
        }
    }
}
