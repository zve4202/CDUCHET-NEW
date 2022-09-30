using DevExpress.XtraGrid.Views.Grid;
using GH.Utils;
using GH.Windows;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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

        internal async void ProcessExcel()
        {
            if (!IsDataReady())
                return;
            BeginProcess();
            int index = 0;

            foreach (XlShablon item in _shablons)
            {
                item.StartProcess(this);
                DoSelect(item);

                CancellationToken cancellationToken = item.CancellationToken;

                await Task.Factory.StartNew(() =>
                {
                    ProcessData(item, index, cancellationToken);
                }, cancellationToken);
                item.ClearExcelData();
                if (cancellationToken.IsCancellationRequested)
                    break;
                index++;

            }
            if (index < _shablons.Count)
                DoSelect(_shablons[index]);
            else
                DoSelect(null);
            EndProcess();

        }



        protected virtual void ProcessData(XlShablon shablon, int index, CancellationToken cancellationToken)
        {
            _shablon = shablon;
            CreateResultTable(index);
            FillResultTable(index, cancellationToken);
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

        XlShablon _shablon;
        protected XlShablon Shablon => _shablon;
        List<string> otherFields = new List<string>();

        protected virtual void CalculateRow(DataRow row, int index)
        {
            throw new NotImplementedException();
        }

        internal void CreateResultTable(int index)
        {
            _shablon.SetStatus("Ждите: идёт подготовка данных...");
            if (ResultData == null)
            {
                ResultData = new DataTable();
            }

            List<FieldParam> mapFields = _shablon.DataMap.ToList();
            List<FieldParam> outsourceFields = mapFields.Where(f => f.ParamFunc == ParamFunctionType.OutSourceData).ToList();

            otherFields = (_shablon.ExcelData.Columns.Cast<DataColumn>().Select(x => x.ColumnName)).ToList();
            int columnIndex = 0;
            foreach (FieldParam field in mapFields)
            {
                if (field.ExcelPanel != null)
                    otherFields.Remove(field.ExcelPanel.Column.ColumnName);

                DataColumn column;
                if (ResultData.Columns.IndexOf(field.Name) == -1)
                {
                    column = new DataColumn(field.Name, field.DataType);
                    if (field.ParamFunc == FieldParam.ParamFunctionType.ToSumm)
                    {
                        column.Caption = field.Caption(index + 1);
                        column.AllowDBNull = true;

                    }
                    else
                        column.Caption = field.Caption();
                    ResultData.Columns.Add(column);
                    ResultData.Columns[field.Name].SetOrdinal(columnIndex + (index));
                    if (field.ParamFunc == FieldParam.ParamFunctionType.IsKey)
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

            if (ResultData.Columns.Count < _shablon.ExcelData.Columns.Count + outsourceFields.Count + index)
            {
                while (ResultData.Columns.Count < _shablon.ExcelData.Columns.Count + outsourceFields.Count + index)
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

        Dictionary<object, DataRow> keys = new Dictionary<object, DataRow>();
        internal void FillResultTable(int index, CancellationToken cancellationToken)
        {
            if (index == 0)
                keys.Clear();
            _shablon.SetStatus("Ждите: идёт обработка данных...");

            List<FieldParam> baseFields = _shablon.DataMap.Where(m => m.ParamFunc != FieldParam.ParamFunctionType.OutSourceData).ToList();
            List<FieldParam> outsourceFields = _shablon.DataMap.Where(m => m.ParamFunc == FieldParam.ParamFunctionType.OutSourceData).ToList();

            if (baseFields.Count > 0)
            {
                DataRow[] excelList = _shablon.ExcelData.Select();
                int pos = 0;
                foreach (DataRow excelRow in excelList)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    DataRow resultRow = null;
                    try
                    {
                        foreach (FieldParam field in baseFields)
                        {
                            object value = field.ExcelValue(excelRow);
                            switch (field.ParamFunc)
                            {
                                case FieldParam.ParamFunctionType.IsKey:
                                    keys.TryGetValue(value, out resultRow);
                                    if (resultRow != null)
                                        continue;
                                    resultRow = ResultData.NewRow();
                                    resultRow.SetField<object>(field.Name, value);
                                    int i = _shablon.DataMap.Count + index;
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
                                case FieldParam.ParamFunctionType.ToSumm:
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

                        if (outsourceFields.Count > 0)
                        {
                            SetOutsourceFieds(resultRow);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error("FillResultTable", e);
                        continue;
                    }



                    //DataRow resultRow = (from line in ResultData.AsEnumerable()
                    //                     where line.Field<string>(barcodeName) == barcode
                    //                     select line).FirstOrDefault();


                    //if (resultRow != null)
                    //{
                    //    try
                    //    {
                    //        qty += resultRow.Field<int>(qtyName);
                    //    }
                    //    catch { }
                    //    resultRow.SetField<int>(qtyName, qty);
                    //}
                    //else
                    //{
                    //    resultRow = ResultData.NewRow();
                    //    resultRow.SetField<string>(barcodeName, barcode);
                    //    resultRow.SetField<int>(qtyName, qty);
                    //    int i = 2 + index;
                    //    foreach (var item in otherFields)
                    //    {
                    //        string val = "";
                    //        try
                    //        {
                    //            val = excelRow.Field<string>(item);
                    //        }
                    //        catch { }
                    //        resultRow.SetField<string>(i, val);
                    //        i++;
                    //    }
                    //    ResultData.Rows.Add(resultRow);
                    //    keys.Add(barcode, resultRow);
                    //}
                    resultRow.AcceptChanges();
                    _shablon.ExcelData.Rows.Remove(excelRow);
                    _shablon.SetProgress(pos++);
                }

                /// заменянм название колонки ParamFunctionType.ToSumm на уникальное 
                /// чтобы потом добавить ещё одну такую же из другого набора (если есть такой)
                foreach (var field in baseFields)
                {
                    switch (field.ParamFunc)
                    {
                        case FieldParam.ParamFunctionType.ToSumm:
                            string columnName = string.Format("{0}{1}", field.Name, index + 1);
                            var column = ResultData.Columns[field.Name];
                            column.ColumnName = columnName;
                            break;
                        default:
                            break;
                    }
                }

                if (baseFields.Count > 1 && baseFields.Count - 1 == index)
                {
                    if (AddCalculateFields(index))
                    {
                        pos = 0;
                        foreach (var row in ResultData.Select())
                        {
                            CalculateRow(row, index);
                            _shablon.SetProgress(pos++);

                        }
                    }
                }

            }
        }

        protected virtual void SetOutsourceFieds(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
