﻿using DevExpress.XtraGrid.Views.Grid;
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
            return new Worker(this, Shablon.CancellationToken, excelRows);
        }

        internal async void ProcessExcel()
        {
            if (!IsDataReady())
                return;
            BeginProcess();

            for (int i = 0; i < _shablons.Count; i++)
            {
                _shablonIndex = i;

                Shablon.StartProcess(this);
                DoSelect(Shablon);

                await Task.Factory.StartNew(() =>
                {
                    ProcessData();
                }, Shablon.CancellationToken);

                Shablon.ClearExcelData();

                if (Shablon.CancellationToken.IsCancellationRequested)
                    break;
            }
            if (_shablonIndex < _shablons.Count)
                DoSelect(Shablon);
            else
                DoSelect(null);
            EndProcess();

        }

        protected virtual void ProcessData()
        {
            CreateResultTable();
            CreateWorkersPull();
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
            //view.OptionsPrint.EnableAppearanceEvenRow = true;
            //view.OptionsPrint.EnableAppearanceOddRow = true;
            //view.OptionsPrint.PrintHorzLines = true;
            //view.OptionsPrint.PrintVertLines = true;

            ControlExecute(() =>
            {
                view.ExportToXlsx(file);
            }, "EXCEL");

            //view.ExportToXlsx(file, new XlsxExportOptions(TextExportMode.Value, false));
            Process.Start(file);
            Application.Exit();
        }

        private int _shablonIndex = 0;
        protected XlShablon Shablon => _shablons.Count == 0 ? null : _shablons[_shablonIndex];
        List<string> otherFields = new List<string>();

        protected virtual void CalculateRow(DataRow row, int index)
        {
            throw new NotImplementedException();
        }

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
                        column.Caption = field.Caption(_shablonIndex + 1);
                        column.AllowDBNull = true;

                    }
                    else
                        column.Caption = field.Caption();
                    ResultData.Columns.Add(column);
                    ResultData.Columns[field.Name].SetOrdinal(columnIndex + (_shablonIndex));
                    if (field.ParamFunc == ParamFunctionType.IsKey)
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

            if (ResultData.Columns.Count < Shablon.ExcelData.Columns.Count + outsourceFields.Count + _shablonIndex)
            {
                while (ResultData.Columns.Count < Shablon.ExcelData.Columns.Count + outsourceFields.Count + _shablonIndex)
                {
                    columnIndex = ResultData.Columns.Count;
                    string columnName = string.Format("column{0}", columnIndex);
                    DataColumn column = new DataColumn(columnName, typeof(string));
                    column.Caption = string.Format("column-{0}", columnIndex);
                    ResultData.Columns.Add(column);
                }
            }
        }

        public virtual void CreateOutsourceMap(FieldsMap dataMap)
        {

        }

        protected virtual bool AddCalculateFields(int index)
        {
            throw new NotImplementedException();
        }

        int currentProcessed = 0;
        int totalForProcess = 0;

        List<FieldParam> baseFields = null;
        List<FieldParam> outsourceFields = null;
        Dictionary<object, DataRow> keys = new Dictionary<object, DataRow>();
        private object locker;

        internal void CreateWorkersPull()
        {
            if (_shablonIndex == 0)
                keys.Clear();
            currentProcessed = 0;

            Shablon.SetStatus("Ждите: идёт обработка данных...");

            baseFields = Shablon.DataMap.Where(m => m.ParamFunc != ParamFunctionType.OutSourceData).ToList();
            outsourceFields = Shablon.DataMap.Where(m => m.ParamFunc == ParamFunctionType.OutSourceData).ToList();

            if (baseFields.Count > 0)
            {
                DataRow[] excelList = Shablon.ExcelData.Select();
                totalForProcess = excelList.Length;
                List<DataRow> workerList = new List<DataRow>();

                foreach (DataRow excelRow in excelList)
                {
                    if (Shablon.CancellationToken.IsCancellationRequested)
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

        }

        public void ProcessRow(DataRow excelRow, object autsourceData = null)
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
                            keys.TryGetValue(value, out resultRow);
                            if (resultRow != null)
                                continue;
                            resultRow = ResultData.NewRow();
                            resultRow.SetField<object>(field.Name, value);
                            int i = Shablon.DataMap.Count + _shablonIndex;
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
                Logger.Error("FillResultTable", e);
            }

            lock (locker)
            {
                currentProcessed++;
                Shablon.SetStatus("Ждите: идёт обработка данных..." +
                    Environment.NewLine +
                    string.Format("Обработано {0} из {1}", currentProcessed, totalForProcess));
            }

        }

        //internal void FillResultTable(CancellationToken cancellationToken)
        //{
        //    /*


        //    if (baseFields.Count > 0)
        //    {
        //        DataRow[] excelList = Shablon.ExcelData.Select();
        //        int pos = 0;
        //        foreach (DataRow excelRow in excelList)
        //        {
        //            if (cancellationToken.IsCancellationRequested)
        //                return;


        //            DataRow resultRow = null;
        //            try
        //            {
        //                foreach (FieldParam field in baseFields)
        //                {
        //                    object value = field.ExcelValue(excelRow);
        //                    switch (field.ParamFunc)
        //                    {
        //                        case FieldParam.ParamFunctionType.IsKey:
        //                            keys.TryGetValue(value, out resultRow);
        //                            if (resultRow != null)
        //                                continue;
        //                            resultRow = ResultData.NewRow();
        //                            resultRow.SetField<object>(field.Name, value);
        //                            int i = Shablon.DataMap.Count + index;
        //                            foreach (string item in otherFields)
        //                            {
        //                                object val = null;
        //                                try
        //                                {
        //                                    val = excelRow.Field<object>(item);
        //                                }
        //                                catch { }
        //                                resultRow.SetField<object>(i, val);
        //                                i++;
        //                            }
        //                            ResultData.Rows.Add(resultRow);
        //                            keys.Add(value, resultRow);
        //                            break;
        //                        case FieldParam.ParamFunctionType.ToSumm:
        //                            if (resultRow.RowState == DataRowState.Added)
        //                                resultRow.SetField<object>(field.Name, value);
        //                            else
        //                            {
        //                                int qty = 0;
        //                                int.TryParse(value.ToString(), out qty);
        //                                try
        //                                {
        //                                    qty += resultRow.Field<int>(field.Name);
        //                                }
        //                                catch { }
        //                                resultRow.SetField<int>(field.Name, qty);
        //                            }
        //                            break;
        //                        default:
        //                            break;
        //                    }
        //                }

        //                if (outsourceFields.Count > 0)
        //                {
        //                    SetOutsourceFieds(resultRow);
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Logger.Error("FillResultTable", e);
        //                continue;
        //            }



        //            //DataRow resultRow = (from line in ResultData.AsEnumerable()
        //            //                     where line.Field<string>(barcodeName) == barcode
        //            //                     select line).FirstOrDefault();


        //            //if (resultRow != null)
        //            //{
        //            //    try
        //            //    {
        //            //        qty += resultRow.Field<int>(qtyName);
        //            //    }
        //            //    catch { }
        //            //    resultRow.SetField<int>(qtyName, qty);
        //            //}
        //            //else
        //            //{
        //            //    resultRow = ResultData.NewRow();
        //            //    resultRow.SetField<string>(barcodeName, barcode);
        //            //    resultRow.SetField<int>(qtyName, qty);
        //            //    int i = 2 + index;
        //            //    foreach (var item in otherFields)
        //            //    {
        //            //        string val = "";
        //            //        try
        //            //        {
        //            //            val = excelRow.Field<string>(item);
        //            //        }
        //            //        catch { }
        //            //        resultRow.SetField<string>(i, val);
        //            //        i++;
        //            //    }
        //            //    ResultData.Rows.Add(resultRow);
        //            //    keys.Add(barcode, resultRow);
        //            //}
        //            resultRow.AcceptChanges();
        //            Shablon.ExcelData.Rows.Remove(excelRow);
        //            Shablon.SetProgress(pos++);
        //        }

        //        /// заменянм название колонки ParamFunctionType.ToSumm на уникальное 
        //        /// чтобы потом добавить ещё одну такую же из другого набора (если есть такой)
        //        foreach (var field in baseFields)
        //        {
        //            switch (field.ParamFunc)
        //            {
        //                case FieldParam.ParamFunctionType.ToSumm:
        //                    string columnName = string.Format("{0}{1}", field.Name, index + 1);
        //                    var column = ResultData.Columns[field.Name];
        //                    column.ColumnName = columnName;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //        if (baseFields.Count > 1 && baseFields.Count - 1 == index)
        //        {
        //            if (AddCalculateFields(index))
        //            {
        //                pos = 0;
        //                foreach (var row in ResultData.Select())
        //                {
        //                    CalculateRow(row, index);
        //                    Shablon.SetProgress(pos++);

        //                }
        //            }
        //        }

        //    }
        //    */
        //}

        //protected virtual void SetOutsourceFieds(DataRow row)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
