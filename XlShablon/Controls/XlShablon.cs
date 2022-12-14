using DevExpress.XtraEditors;
using ExcelDataReader;
using GH.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GH.Utils.AppHelper;
using static GH.Utils.FileHelper;
using static GH.Windows.WindowHelper;

namespace GH.XlShablon
{
    public partial class XlShablon : XtraUserControl
    {
        #region Поля и свойства

        private CancellationTokenSource cts = new CancellationTokenSource();
        internal CancellationTokenSource Cts
        {
            get => cts;
            set
            {
                if (cts != null)
                    cts.Dispose();
                cts = value;
            }
        }

        [Browsable(false)]
        public CancellationToken CancellationToken => cts.Token;

        [Browsable(false)]
        public DataProcessor DataProcessor
        {
            get
            {
                if (_dataProcessor == null)
                {
                    Control ctrl = this;
                fromBegin:
                    if (ctrl != null)
                    {
                        ctrl = ctrl.Parent;
                        if (ctrl is IDataProcessor)
                        {
                            _dataProcessor = new DataProcessor(ctrl);

                        }
                        else
                            goto fromBegin;
                    }
                }

                return _dataProcessor;
            }
            set
            {
                _dataProcessor = value;
                if (_dataProcessor != null)
                {
                    _dataProcessor.AddShablon(this);
                }
            }
        }

        [Category("XlShablon")]
        public Control ExtMenuControl
        {
            get => _extMenuControl;
            set
            {
                if (_extMenuControl != null)
                {
                    _extMenuControl.Parent = Parent;
                    _extMenuControl.Dock = DockStyle.None;

                }
                _extMenuControl = value;
                if (_extMenuControl != null)
                {
                    _extMenuControl.Parent = panelMenus;
                    _extMenuControl.Dock = DockStyle.Top;
                    _extMenuControl.BringToFront();
                    RefreshControlsState();
                }
            }
        }
        private Control _extMenuControl;

        [Category("XlShablon")]
        public bool RemoveNotUsedFields { get; set; } = true;

        private string _fileName;
        DataTable _excelData;
        [Browsable(false)]
        public DataTable ExcelData
        {
            get => _excelData;
            internal set
            {
                if (_excelData != null)
                {
                    _excelData.Dispose();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    _excelData = null;
                }

                _excelData = value;
            }
        }


        ExcelMap excelMap = new ExcelMap();

        [Browsable(false)]
        public FieldsMap DataMap { get => _dataMap; }
        private FieldsMap _dataMap = null;

        protected virtual FieldsMap getDataMap()
        {
            return new FieldsMap(this);
        }


        private bool isProcessing = false;

        [Browsable(false)]
        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName == value)
                    return;
                _fileName = value;
            }
        }

        #endregion


        public XlShablon()
        {
            _dataMap = getDataMap();
            InitializeComponent();
            AllowDrop = true;
            CreateParamSlots();
            Disposed += XlShablon_Disposed;
            RefreshControlsState();
        }

        #region Методы
        private void XlShablon_Disposed(object sender, EventArgs e)
        {
            Cts.Dispose();
        }

        private void XlShablon_Load(object sender, EventArgs e)
        {
            panelProc.Width = ((ClientSize.Width - trackBar.Width - splitter1.Width - 10) / 2);
        }

        private void CreateParamSlots()
        {
            foreach (var item in DataMap)
            {
                item.Parent = panelProc;
                item.BringToFront();
            }
        }

        int processStep = 10;
        internal void StartProcess(DataProcessor pocessor)
        {
            pocessor.CreateOutsourceMap(DataMap);

            processStep = Math.Max(1, (ExcelData.Rows.Count / 100));
            isProcessing = true;
            this.InvokeIfRequired(() =>
            {
                RefreshControlsState();
                progressBar.Visible = true;
                progressBar.Position = 0;
                progressBar.Properties.Maximum = ExcelData.Rows.Count;
            });

        }

        public async void LoadFromExcel()
        {
            Clear();
            SetStatus("Ждите: идет загрузка данных...");
            isProcessing = true;
            RefreshControlsState();

            ExcelData = new DataTable();
            Cts = new CancellationTokenSource();
            try
            {
                Task loadInfo = new Task(() =>
                {
                    var info = labelState.Text;
                    while (isProcessing && !CancellationToken.IsCancellationRequested)
                    {
                        if (ExcelData.Rows.Count == 0)
                            Thread.Sleep(500);
                        else
                            Thread.Sleep(250);

                        if (ExcelData != null)
                        {

                            if (CancellationToken.IsCancellationRequested)
                            {


                                SetStatus("Остановка...");
                            }
                            else
                                if (ExcelData.Rows.Count == 0)
                            {
                                if (labelState.Text == info)
                                    SetStatus("");
                                else
                                    SetStatus(info);
                            }
                            else
                                SetStatus(info + Environment.NewLine + $"Загружено {ExcelData.Rows.Count} записей...");
                        };
                    }

                    if (!CancellationToken.IsCancellationRequested)
                        SetStatus($"Загружено {ExcelData.Rows.Count} записей...");

                }, CancellationToken);


                loadInfo.Start();
                await Task.Factory.StartNew(() =>
                {
                    LoadData();
                    isProcessing = false;
                }, CancellationToken);

            }
            finally
            {
                if (CancellationToken.IsCancellationRequested)
                {
                    Clear();
                    ExcelData = null;
                }
                else
                {
                    Reset();
                    SetStatus($"Загружено {ExcelData.Rows.Count} записей...");
                }
                RefreshControlsState();
            }
        }

        private void Reset()
        {
            trackBar.Properties.Maximum = ExcelData.Rows.Count - 1;
            trackBar.Properties.LargeChange = Math.Max(5, ExcelData.Rows.Count / 100);
            trackBar.Properties.Minimum = 0;
            trackBar.Value = 0;
            panelExcel.SuspendLayout();
            for (int i = 0; i < ExcelData.Columns.Count; i++)
            {

                ExcelPanel infoPanel = new ExcelPanel(i, ExcelData.Columns[i]);
                infoPanel.Parent = panelExcel;
                infoPanel.BringToFront();
                excelMap.Add(infoPanel);
            }
            panelExcel.ResumeLayout();
            trackBar.EditValueChanged += TrackBar_EditValueChanged;
            FillXlMap();
            labelState.BringToFront();
        }

        private void TrackBar_EditValueChanged(object sender, EventArgs e)
        {
            FillXlMap();
        }

        private void FillXlMap()
        {
            foreach (var item in excelMap)
            {
                DataRow row = ExcelData.Rows[(int)trackBar.EditValue];
                item.Text = row[item.Column].ToString();
            }
        }

        private void Clear()
        {
            trackBar.EditValueChanged -= TrackBar_EditValueChanged;
            trackBar.Properties.Maximum = 0;
            panelExcel.SuspendLayout();
            DataMap.Clear();
            excelMap.Clear();
            SetStatus("Нет данных...");
            panelExcel.ResumeLayout();
            Refresh();
        }

        private void SetColumnsOrder()
        {
            int columnIndex = 0;
            foreach (var columnName in DataMap.Select(x => x.Name).ToArray())
            {
                ExcelData.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }

        protected void LoadData()
        {
            using (var stream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (IExcelDataReader excelReader = GetExcelReader(stream))
                {
                    FillDataTable(excelReader, new ExcelDataTableConfiguration()
                    {
                        EmptyColumnNamePrefix = "Field",
                        //FilterRow = rowReader => i++ < 300,
                        UseHeaderRow = true
                    });

                    excelReader.Close();
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private IExcelDataReader GetExcelReader(FileStream stream)
        {
            IExcelDataReader excelReader;
            if (Path.GetExtension(_fileName).ToUpper() == ".XLS")
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            if (Path.GetExtension(_fileName).ToUpper() == ".XLSX")
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            else
            {
                excelReader = ExcelReaderFactory.CreateCsvReader(stream);
            }

            return excelReader;
        }

        private void FillDataTable(IExcelDataReader reader, ExcelDataTableConfiguration configuration)
        {
            ExcelData.TableName = reader.Name;
            ExcelData.ExtendedProperties.Add("visiblestate", reader.VisibleState);
            var first = true;
            var columnIndices = new List<int>();
            while (reader.Read())
            {
                if (CancellationToken.IsCancellationRequested)
                    return;

                if (first)
                {
                    if (configuration.UseHeaderRow && configuration.ReadHeaderRow != null)
                    {
                        configuration.ReadHeaderRow(reader);
                    }

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        if (configuration.FilterColumn != null && !configuration.FilterColumn(reader, i))
                        {
                            continue;
                        }

                        var name = configuration.UseHeaderRow
                            ? Convert.ToString(reader.GetValue(i))
                            : null;

                        if (string.IsNullOrEmpty(name))
                        {
                            name = configuration.EmptyColumnNamePrefix + i;
                        }

                        var columnName = GetUniqueColumnName(ExcelData, name);
                        var column = new DataColumn(columnName, typeof(object)) { Caption = name };
                        ExcelData.Columns.Add(column);
                        columnIndices.Add(i);
                    }

                    ExcelData.BeginLoadData();
                    first = false;

                    if (configuration.UseHeaderRow)
                    {
                        continue;
                    }
                }

                if (configuration.FilterRow != null && !configuration.FilterRow(reader))
                {
                    break;
                }

                if (IsEmptyRow(reader))
                {
                    break;
                }

                var row = ExcelData.NewRow();

                for (var i = 0; i < columnIndices.Count; i++)
                {
                    var columnIndex = columnIndices[i];
                    var value = reader.GetValue(columnIndex);
                    row[i] = value;
                }

                ExcelData.Rows.Add(row);
            }

            ExcelData.EndLoadData();
            FixDataTypes();
        }

        private void FixDataTypes()
        {
            bool convert = false;
            DataTable newTable = null;
            for (int i = 0; i < ExcelData.Columns.Count; i++)
            {
                Type type = null;
                foreach (DataRow row in ExcelData.Rows)
                {
                    if (row.IsNull(i))
                        continue;
                    var curType = row[i].GetType();
                    if (curType != type)
                    {
                        if (type == null)
                        {
                            type = curType;
                        }
                        else
                        {
                            type = null;
                            break;
                        }
                    }
                }

                if (type == null)
                    continue;
                convert = true;
                if (newTable == null)
                    newTable = ExcelData.Clone();
                newTable.Columns[i].DataType = type;
            }

            if (newTable != null)
            {
                newTable.BeginLoadData();

                foreach (DataRow row in ExcelData.Rows)
                {
                    newTable.ImportRow(row);
                }

                newTable.EndLoadData();

                if (convert)
                {
                    ExcelData = newTable;
                }
            }

        }

        private string GetUniqueColumnName(DataTable table, string name)
        {
            var columnName = name;
            var i = 1;
            while (table.Columns[columnName] != null)
            {
                columnName = string.Format("{0}_{1}", name, i);
                i++;
            }

            return columnName;
        }

        private bool IsEmptyRow(IExcelDataReader reader)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetValue(i) != null)
                    return false;
            }

            return true;
        }

        internal void RefreshControlsState()
        {
            loadButton.Enabled = !isProcessing;
            stopButton.Enabled = isProcessing;
            clearButton.Enabled = !isProcessing && DataMap.Any(x => x.ExcelPanel != null);
            acceptButton.Enabled = !isProcessing && ExcelData != null;
            if (_extMenuControl is IExtMenuControl intf)
            {
                intf.SetVisible(ExcelData != null);
                intf.SetReadOnly(isProcessing);

            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            trackBar.Focus();
            cts?.Cancel();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            trackBar.Focus();
            DataMap.Clear();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            trackBar.Focus();
            if (string.IsNullOrEmpty(openXlDialog.InitialDirectory))
            {
                openXlDialog.InitialDirectory = Application.StartupPath;
            }
            if (openXlDialog.ShowDialog() == DialogResult.OK)
            {
                _fileName = openXlDialog.FileName;
                LoadFromExcel();
            }
        }

        private DataProcessor _dataProcessor;

        public bool MapIsReady()
        {
            if (!DataMap.MapIsReady(false))
            {

                if (_dataProcessor.DoSelect(this))
                {
                    DataMap.MapIsReady(true);
                    if (ExcelData == null)
                    {
                        loadButton.PerformClick();
                    }

                    return false;
                }

            }
            return true;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            trackBar.Focus();
            Cts = new CancellationTokenSource();
            DataProcessor.ProcessExcel();
        }

        public void SetStatus(string text)
        {
            this.InvokeIfRequired(() =>
            {
                labelState.Text = text;
            });
            Application.DoEvents();
        }

        public void SetProgress(int current = 0)
        {
            if (current % processStep == 0 || current == progressBar.Properties.Maximum)
            {
                this.InvokeIfRequired(() =>
                {
                    progressBar.Position = current + 1;
                    progressBar.Position = current;
                });
                Application.DoEvents();
            }
        }

        public bool PrepareTableForTest()
        {
            DataProcessor.DoSelect(this);
            if (!DataMap.MapIsReady(true))
                return false;

            SetStatus("Ждите: идёт подготовка данных...");

            foreach (var item in DataMap)
            {
                var xl = excelMap.Where(x => x.Column.ColumnName == item.Name).FirstOrDefault();
                if (xl != null)
                    xl.Column.ColumnName = GetUniqueColumnName(ExcelData, item.Name);

                DataColumn column = null;

                if (item.ExcelPanel == null || item.DataType != item.ExcelPanel.DataType)
                {
                    column = new DataColumn(item.Name, item.DataType);
                    ExcelData.Columns.Add(column);
                }
                else
                if (DataMap.Any(x => x.Column == item.ExcelPanel.Column))
                {
                    column = new DataColumn(item.Name, item.DataType);
                    ExcelData.Columns.Add(column);
                }
                else
                {
                    column = item.ExcelPanel.Column;
                    column.ColumnName = item.Name;
                }

                column.Caption = item.Caption();
                item.Column = column;
            }

            SetColumnsOrder();

            return ExcelData != null;
        }

        public void TestTable()
        {
            List<FieldParam> list = DataMap.Where(data => data.NeedToFill).ToList();
            if (list.Count > 0)
            {
                DataRow[] excelList = ExcelData.Select();
                foreach (var row in excelList)
                {
                    foreach (var item in list)
                    {
                        item.SetColumnValue(row);
                    }
                }
            }


            if (RemoveNotUsedFields)
            {
                int last = DataMap.Count;
                while (ExcelData.Columns.Count > last)
                {
                    ExcelData.Columns.RemoveAt(last);
                }
            }
            else
            {
                foreach (var item in DataMap)
                {
                    item.RemoveSourceColumn();
                }
            }
        }

        internal void ClearExcelData()
        {
            if (!CancellationToken.IsCancellationRequested)
            {
                ControlExecute(() =>
                {
                    MoveFile(ProcessedFolder, FileName);
                }, "EXCEL");
            }
            isProcessing = false;
            progressBar.Visible = false;
            Clear();
            ExcelData = null;
            if (CancellationToken.IsCancellationRequested)
                SetStatus("Отмена...");
            else
                SetStatus("Готово...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Application.DoEvents();
            RefreshControlsState();
        }

        #endregion
    }
}
