using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GH.XlShablon.Workers
{
    public static class WorkersPull
    {
        private const int workersCount = 4;
        private static int currentWorker = 1;
        public static int MaxWorkerLine = 100;
        public static int CurrentWorker
        {
            get
            {
                int result = currentWorker;
                currentWorker++;
                if (currentWorker > workersCount)
                    currentWorker = 1;
                return result;
            }
        }

        private static List<Worker> workersPull = null;
        private static List<Worker> workers = null;


        private static async void Execute()
        {
            if (workers == null)
            {
                workers = new List<Worker>();
                await Task.Factory.StartNew(() =>
                {
                    while (workersPull.Count > 0 || workers.Count > 0)
                    {
                        if (workers.Count < workersCount && workersPull.Count > 0)
                        {
                            var worker = workersPull.First();
                            workersPull.Remove(worker);
                            if (worker.CanWork)
                            {
                                workers.Add(worker);
                                worker.Execute();
                                Thread.Sleep(500);
                            }
                        }
                    }
                });
                workers = null;
                workersPull = null;
                currentWorker = 1;
            }
        }

        public static void AddToPool(Worker worker)
        {
            if (workersPull == null)
                workersPull = new List<Worker>();
            workersPull.Add(worker);
            Execute();
        }

        public static void RemoveFromPool(Worker worker)
        {
            workers.Remove(worker);
        }

    }
}
