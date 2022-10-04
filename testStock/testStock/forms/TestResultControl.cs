using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using GH.Windows;
using GH.XlShablon;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GH.Windows.DlgHelper;

namespace Tester.forms
{
    public partial class TestResultControl : XtraUserControl
    {
        private DataProcessor _dataProcessor;
        private bool _saved = false;


        public TestResultControl()
        {
            InitializeComponent();
            btnSave.Enabled = _dataProcessor != null && _dataProcessor.ResultData.Rows.Count > 0;
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_saved && _dataProcessor != null && _dataProcessor.ResultData.Rows.Count > 0)
            {
                if (DlgYesNo("Желаете сохранить в Excel?"))
                {
                    e.Cancel = true;
                    Task.Delay(100).ContinueWith((task) =>
                    {
                        Save();
                    });
                }
            }
        }

        public void SetResult(DataProcessor dataProcessor)
        {
            ParentForm.FormClosing -= ParentForm_FormClosing;
            ParentForm.FormClosing += ParentForm_FormClosing;

            _dataProcessor = dataProcessor;
            mainView.Columns.Clear();

            gridResult.DataSource = _dataProcessor.ResultData;
            foreach (GridColumn item in mainView.Columns)
            {
                if (item.Name.ToLower().Contains("barcode"))
                {
                    item.Summary.Add(
                        new GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "", "Записей: {0:0}")
                    );
                }
                if (item.Name.ToLower().Contains("qty"))
                {
                    item.Summary.Add(
                        new GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "", "{0:0}")
                    );
                }
            }
            ParentForm.WindowState = FormWindowState.Maximized;
            Application.DoEvents();
            mainView.BestFitColumns(true);
            btnSave.Enabled = _dataProcessor.ResultData.Rows.Count > 0;
        }

        private void Save()
        {
            this.InvokeIfRequired(() =>
            {
                _saved = true;
                _dataProcessor.ExportToXl(mainView);
            });
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            Save();
        }
    }
}
