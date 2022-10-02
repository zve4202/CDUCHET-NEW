using DevExpress.XtraEditors;
using GH.Database;
using GH.XlShablon;
using System.Collections.Generic;
using System.Linq;
using Tester.Database;
using static GH.Windows.DevExpressHelper;

namespace Tester.forms
{
    public partial class ExcelDbProcSetting : XtraUserControl, IExtMenuControl
    {
        private static ExcelDbProcSetting _setting;
        public static ExcelDbProcSetting Setting => _setting;

        public ExcelDbProcSetting()
        {
            InitializeComponent();
            Reset();
            SetupLookups(this);
            _setting = this;
        }

        internal TestParams GetTestParams(string barcode)
        {
            return new TestParams(
                comboType.SelectedIndex,
                barcode,
                comboStock.SelectedIndex,
                (int)(comboClients.EditValue ?? 0)
             );
        }


        public INHRepository IClients { get; internal set; }

        public void Reset()
        {
            comboType.SelectedIndex = 0;
            comboStock.SelectedIndex = 0;
            comboClients.EditValue = 0;
            UpdateControls();
        }

        public void SetReadOnly(bool value)
        {
            comboType.ReadOnly = value;
            comboStock.ReadOnly = value;
            comboClients.ReadOnly = value || comboType.SelectedIndex == 2;
        }

        public void SetVisible(bool value)
        {
            Visible = value || DesignMode;
            if (Visible && !DesignMode)
            {
                Dictionary<int, string> clients = IClients.KeyIntLookupList();
                clients.Add(0, "== ВСЕ РЕАЛИЗАТОРЫ");
                comboClients.EditValue = 0;
                comboClients.Properties.DataSource = clients.ToArray();
            }
        }


        private void comboType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            panelClients.Visible = comboType.SelectedIndex == 1 || comboType.SelectedIndex == 2 || DesignMode;
            comboClients.ReadOnly = comboType.SelectedIndex == 2;
            if (comboType.SelectedIndex == 2)
            {
                comboClients.EditValue = 0;
            }
        }

    }
}
