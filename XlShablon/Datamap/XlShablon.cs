using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using DevExpress.Spreadsheet;

namespace GH.XlShablon
{
    public partial class XlShablon : XtraUserControl
    {
        private string _fileName;
        Workbook workbook = new Workbook();
        Worksheet worksheet;
        ExcelMap excelMap = new ExcelMap();
        ParamMap paramMap = new ParamMap();

        int _firstCol = 0;
        int _firstRow = 0;
        int _lastCol = 0;
        int _lastRow = 0;

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
        public XlShablon()
        {
            InitializeComponent();
            AllowDrop = true;
            CreateParamSlots();
            workbook.DocumentLoaded += Workbook_DocumentLoaded;
        }

        private void CreateParamSlots()
        {
            foreach (var item in paramMap)
            {
                item.Parent = panelProc;
                item.BringToFront();
            }            
        }

        private void Workbook_DocumentLoaded(object sender, EventArgs e)
        {
            worksheet = workbook.Worksheets.ActiveWorksheet;

            Range range = worksheet.GetUsedRange();
            _firstCol = range.LeftColumnIndex;
            int max = ('Z' - 'A') + 1;
            _lastCol = Math.Min(range.RightColumnIndex, max);
            _firstRow = range.TopRowIndex;
            _lastRow = range.BottomRowIndex;
        }

        public void InvokeIfRequired(MethodInvoker action)
        {
            if (IsDisposed)
                return;

            try
            {
                if (InvokeRequired)
                    Invoke(action);
                else
                    action.Invoke();
            }
            catch { }
        }

        public async void Load()
        {
            Clear();
            await Task.Factory.StartNew(() =>
            {
                workbook.LoadDocument(Path.Combine(Application.StartupPath, _fileName));
                InvokeIfRequired(() => Reset());
            });
        }

        private void Reset()
        {
            trackBar.Properties.Maximum = _lastRow;
            trackBar.Properties.Minimum = _firstRow;
            trackBar.Value = _firstRow;
            panelExcel.SuspendLayout();
            for (int i = _firstCol; i <= _lastCol; i++)
            {

                ExcelPanel infoPanel = new ExcelPanel(i);
                infoPanel.Parent = panelExcel;
                infoPanel.BringToFront();
                excelMap.Add(infoPanel);
            }
            panelExcel.ResumeLayout();
            trackBar.EditValueChanged += TrackBar_EditValueChanged;
            FillXlMap();

        }

        private void TrackBar_EditValueChanged(object sender, EventArgs e)
        {
            FillXlMap();
        }

        private void FillXlMap()
        {
            foreach (var item in excelMap)
            {
                var cell = worksheet.GetCellValue(item.Col, (int)trackBar.EditValue);
                string xlText = cell.TextValue;
                if (cell.IsDateTime)
                    xlText = cell.DateTimeValue.ToString("d");
                else
                if (cell.IsNumeric)
                    xlText = cell.NumericValue.ToString();

                item.Text = xlText;
            }
        }

        private void Clear()
        {
            trackBar.EditValueChanged -= TrackBar_EditValueChanged;
            panelExcel.SuspendLayout();
            excelMap.Clear();
            panelExcel.ResumeLayout();

            trackBar.Properties.Maximum = 0;
        }

        public bool GetDataTable(ref DataTable table)
        {
            if (!MapReady())
                return false;

            if (table == null )
                table = new DataTable();

            table.Clear();
            table.Columns.Clear();

            foreach (var item in paramMap)
            {
                DataColumn column = new DataColumn(item.Name, item.ParamType);
                column.AllowDBNull = true;
                if (item.ParamType == typeof(string))
                    column.MaxLength = item.Length;
                table.Columns.Add(column);
            }

            DataRow row = table.NewRow();

            return true;
        }

        private bool MapReady()
        {
            return true;
        }
    }
}
