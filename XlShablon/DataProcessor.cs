using DevExpress.XtraGrid.Views.Grid;
using GH.Database;
using GH.Utils;
using GH.Windows;
using GH.XlShablon.Workers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GH.Windows.WindowHelper;
using static GH.XlShablon.FieldParam;


namespace GH.XlShablon
{
    public enum ProcessScanType { Unique, AsIs };
    public class DataProcessor
    {
        public DataProcessor(Control control)
        {
            if (control is IDataProcessor process)
                intf = process;
        }

        List<XlShablon> _shablons = new List<XlShablon>();
        protected IDataProcessor intf;

        public DataTable ResultData
        {
            get => _resultData;
            internal set
            {
                if (_resultData != null)
                {
                    _resultData.Dispose();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    _resultData = null;
                }

                _resultData = value;
            }
        }

        public object GetKeyValue(DataRow excelRow)
        {
            return Shablon.DataMap.Where(m => m.ParamFunc == ParamFunctionType.IsKey).First().ExcelValue(excelRow);
        }

        private DataTable _resultData;


        internal void AddShablon(XlShablon xlShablon)
        {
            if (_shablons.Contains(xlShablon))
                return;
            _shablons.Add(xlShablon);
        }

        private bool IsDataReady()
        {
            foreach (var item in _shablons)
            {
                if (!item.MapIsReady())
                    return false;

            }

            return true;
        }


        public virtual string ResultFileName
        {
            get
            {
                string fileName = _shablons.Count == 1 ? "result_" : "";
                for (int i = 0; i < _shablons.Count; i++)
                {
                    string name = Path.GetFileNameWithoutExtension(_shablons[i].FileName);
                    if (i == 0)
                        fileName += name;
                    else
                        fileName += "-" + name;
                }
                return fileName;
            }
        }


        protected virtual void BeginProcess()
        {
            ResultData = null;
            if (intf != null)
                intf.BeginProcess();
        }

        protected virtual void EndProcess()
        {
            if (intf != null)
                intf.EndProcess();
        }

        protected virtual Worker GetWorker(DataRow[] excelRows)
        {
            return new Worker(this, excelRows);
        }

        internal async void ProcessExcel()
        {
            if (!IsDataReady())
                return;
            BeginProcess();

            for (int i = 0; i < _shablons.Count; i++)
            {
                ShablonIndex = i;

                Shablon.StartProcess(this);
                DoSelect(Shablon);
                await ProcessData();
                Shablon.ClearExcelData();

                if (IsCancellationRequested)
                    break;
            }

            if (ShablonIndex < _shablons.Count - 1)
                DoSelect(Shablon);
            else
                DoSelect(null);
            EndProcess();

        }

        protected Task ProcessData()
        {

            CreateResultTable();
            return CreateWorkersPull();
        }

        internal bool DoSelect(XlShablon shablon)
        {
            if (intf != null)
            {
                intf.GetControl().InvokeIfRequired(() =>
                {
                    if (shablon == null)
                        intf.SelectResult();
                    else
                        intf.SelectShablon(shablon);
                    Application.DoEvents();
                });
                return true;
            }
            return false;
        }

        public void ExportToXl(GridView view)
        {
            string file = Path.Combine(AppHelper.StartupPath, AppHelper.ExportFolder, ResultFileName) + ".xlsx";

            ControlExecute(() =>
            {
                view.ExportToXlsx(file);
            }, "EXCEL");

            Process.Start(file);
            Application.Exit();
        }

        private int _shablonIndex = 0;
        protected int ShablonIndex
        {
            get { return _shablonIndex; }
            private set { _shablonIndex = value; }
        }
        protected XlShablon Shablon => _shablons.Count == 0 ? null : _shablons[ShablonIndex];
        List<string> otherFields = new List<string>();

        protected virtual void CalculateRow(DataRow row)
        {
            throw new NotImplementedException();
        }

        protected virtual ProcessScanType ProcessScanType => ProcessScanType.Unique;

        public bool IsCancellationRequested => Shablon != null ? Shablon.IsCancellationRequested : true;

        internal void CreateResultTable()
        {
            Shablon.SetStatus("Ждите: идёт подготовка данных...");
            if (ResultData == null)
            {
                ResultData = new DataTable();
            }

            List<FieldParam> mapFields = Shablon.DataMap.ToList();
            List<FieldParam> outsourceFields = mapFields.Where(f => f.ParamFunc == ParamFunctionType.OutSourceData).ToList();

            otherFields = (Shablon.ExcelData.Columns.Cast<DataColumn>().Select(x => x.ColumnName)).ToList();
            int columnIndex = 0;
            foreach (FieldParam field in mapFields)
            {
                if (field.ExcelPanel != null)
                    otherFields.Remove(field.ExcelPanel.Column.ColumnName);

                DataColumn column;
                if (ResultData.Columns.IndexOf(field.Name) == -1)
                {
                    column = new DataColumn(field.Name, field.DataType);
                    if (field.ParamFunc == ParamFunctionType.ToSumm)
                    {
                        column.Caption = field.Caption(ShablonIndex + 1);
                        column.AllowDBNull = true;

                    }
                    else
                        column.Caption = field.Caption();
                    ResultData.Columns.Add(column);
                    ResultData.Columns[field.Name].SetOrdinal(columnIndex + ShablonIndex);
                    if (field.ParamFunc == ParamFunctionType.IsKey && ProcessScanType == ProcessScanType.Unique)
                    {
                        column.AllowDBNull = false;
                        column.Unique = true;
                        var primaryKey = new DataColumn[1];
                        primaryKey[0] = column;
                        ResultData.PrimaryKey = primaryKey;
                    }
                }
                columnIndex++;
            }

            if (ResultData.Columns.Count < Shablon.ExcelData.Columns.Count + outsourceFields.Count + ShablonIndex)
            {
                if (ProcessScanType == ProcessScanType.AsIs)
                {
                    foreach (string fieldName in otherFields)
                    {
                        DataColumn excelColumn = Shablon.ExcelData.Columns[fieldName];
                        Type resultType = typeof(string);
                        if (excelColumn.DataType == typeof(int))
                            resultType = excelColumn.DataType;
                        else if (excelColumn.DataType == typeof(double))
                        {
                            if (Shablon.ExcelData.Select().Any((row) => row.Field<double>(fieldName) % 1 > 0))
                                resultType = excelColumn.DataType;
                            else
                                resultType = typeof(int);
                        }


                        columnIndex = ResultData.Columns.Count;
                        string columnName = string.Format("column{0}", columnIndex);
                        DataColumn resultColumn = new DataColumn(columnName, resultType);
                        resultColumn.Caption = string.Format("column-{0}", columnIndex);
                        ResultData.Columns.Add(resultColumn);
                    }
                }
                else
                {
                    while (ResultData.Columns.Count < Shablon.ExcelData.Columns.Count + outsourceFields.Count + ShablonIndex)
                    {
                        columnIndex = ResultData.Columns.Count;
                        string columnName = string.Format("column{0}", columnIndex);
                        DataColumn column = new DataColumn(columnName, typeof(string));
                        column.Caption = string.Format("column-{0}", columnIndex);
                        ResultData.Columns.Add(column);
                    }
                }
            }
        }

        public virtual void CreateOutsourceMap(FieldsMap dataMap)
        {

        }

        protected virtual bool AddCalculateFields()
        {
            throw new NotImplementedException();
        }

        List<FieldParam> baseFields = null;
        List<FieldParam> outsourceFields = null;
        Dictionary<object, DataRow> keys = new Dictionary<object, DataRow>();

        internal Task CreateWorkersPull()
        {
            if (ShablonIndex == 0)
                keys.Clear();

            Shablon.SetStatus("Ждите: идёт обработка данных...");

            baseFields = Shablon.DataMap.Where(m => m.ParamFunc != ParamFunctionType.OutSourceData).ToList();
            outsourceFields = Shablon.DataMap.Where(m => m.ParamFunc == ParamFunctionType.OutSourceData).ToList();

            return Task.Factory.StartNew(() =>
            {
                if (baseFields.Count > 0)
                {
                    DataRow[] excelList = Shablon.GetExcelList();
                    List<DataRow> workerList = new List<DataRow>();

                    foreach (DataRow excelRow in excelList)
                    {
                        if (IsCancellationRequested)
                            return;
                        workerList.Add(excelRow);
                        if (workerList.Count == WorkersPull.MaxWorkerLine)
                        {
                            Worker worker = GetWorker(workerList.ToArray());
                            workerList.Clear();
                        }
                    }

                    if (workerList.Count > 0)
                    {
                        Worker worker = GetWorker(workerList.ToArray());
                        workerList.Clear();
                    }
                }

                Shablon.WaitForEnd();

                foreach (var field in baseFields)
                {
                    switch (field.ParamFunc)
                    {
                        case ParamFunctionType.ToSumm:
                            string columnName = string.Format("{0}{1}", field.Name, ShablonIndex + 1);
                            var column = ResultData.Columns[field.Name];
                            column.ColumnName = columnName;
                            break;
                        default:
                            break;
                    }
                }

                if (baseFields.Count > 1 && baseFields.Count - 1 == ShablonIndex)
                {
                    if (AddCalculateFields())
                    {
                        foreach (var row in Shablon.GetDataList(ResultData))
                        {
                            CalculateRow(row);
                            Shablon.SetNextStep();
                        }
                    }
                }
            });
        }

        private object addRowLocker = new object();
        public void ProcessRow(DataRow excelRow, object autsourceData = null)
        {

            lock (addRowLocker)
            {
                DataRow resultRow = null;
                try
                {
                    foreach (FieldParam field in baseFields)
                    {
                        object value = field.ExcelValue(excelRow);
                        switch (field.ParamFunc)
                        {
                            case ParamFunctionType.IsKey:
                                if (ProcessScanType == ProcessScanType.Unique)
                                {
                                    keys.TryGetValue(value, out resultRow);
                                    if (resultRow != null)
                                        continue;
                                }
                                resultRow = ResultData.NewRow();
                                resultRow.SetField<object>(field.Name, value);
                                int i = Shablon.DataMap.Count + ShablonIndex;
                                foreach (string item in otherFields)
                                {
                                    object val = null;
                                    try
                                    {
                                        val = excelRow.Field<object>(item);
                                    }
                                    catch { }
                                    resultRow.SetField<object>(i, val);
                                    i++;
                                }
                                ResultData.Rows.Add(resultRow);
                                keys.Add(value, resultRow);
                                break;
                            case ParamFunctionType.ToSumm:
                                if (resultRow.RowState == DataRowState.Added)
                                    resultRow.SetField<object>(field.Name, value);
                                else
                                {
                                    int qty = 0;
                                    int.TryParse(value.ToString(), out qty);
                                    try
                                    {
                                        qty += resultRow.Field<int>(field.Name);
                                    }
                                    catch { }
                                    resultRow.SetField<int>(field.Name, qty);
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    if (autsourceData is ITestResult testResult)
                    {
                        foreach (PropertyInfo prop in testResult.GetScanTypeProperties())
                        {
                            resultRow.SetField<object>(prop.Name, prop.GetValue(autsourceData));
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(nameof(ProcessRow), e);
                }
            }
            Shablon.SetNextStep();
        }
    }
}
