using DevExpress.XtraEditors;
using GH.XlShablon;
using System.Windows.Forms;

namespace Tester.forms
{
    public partial class ExcelVsExcelControl : XtraUserControl, IDataProcessor
    {
        private ExcelVsExcelProcessor dataProcessor;
        private bool _canPage = true;

        public ExcelVsExcelControl()
        {
            InitializeComponent();
            pages.SelectedTabPage = excel_1_Page;
            dataProcessor = new ExcelVsExcelProcessor(this);
            excel_1.DataProcessor = dataProcessor;
            excel_2.DataProcessor = dataProcessor;
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
                else if (shablon == excel_2)
                {
                    pages.SelectedTabPage = excel_2_Page;
                    excel_2.Select();
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
