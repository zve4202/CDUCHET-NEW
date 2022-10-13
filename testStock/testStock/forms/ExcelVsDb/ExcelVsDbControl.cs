using DevExpress.XtraEditors;
using GH.XlShablon;
using System.Windows.Forms;

namespace Tester.forms
{
    public partial class ExcelVsDbControl : XtraUserControl, IDataProcessor
    {
        private ExcelVsDbProcessor dataProcessor;
        private bool _canPage = true;

        public ExcelVsDbControl()
        {

            InitializeComponent();
            pages.SelectedTabPage = excel_1_Page;
            dataProcessor = new ExcelVsDbProcessor(this);
            dataProcessor.ProcSetting = excelVsDbSetting;
            excel_1.DataProcessor = dataProcessor;
        }

        public void BeginProcess()
        {
            _canPage = false;
        }

        public void EndProcess()
        {
            _canPage = true;
        }

        public Control GetControl()
        {
            return this;
        }

        public void SelectResult()
        {
            if (Disposing || IsDisposed)
                return;

            _canPage = true;
            excelResult.SetResult(dataProcessor);
            pages.SelectedTabPage = resultPage;
        }

        public void SelectShablon(XlShablon shablon)
        {
            bool savedCanPage = _canPage;
            _canPage = true;
            try
            {
                if (shablon == excel_1)
                {
                    pages.SelectedTabPage = excel_1_Page;
                    excel_1.Select();
                }

            }
            finally
            {
                _canPage = savedCanPage;
            }
        }

        private void pages_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            if (!_canPage)
                e.Cancel = true;
        }
    }
}
