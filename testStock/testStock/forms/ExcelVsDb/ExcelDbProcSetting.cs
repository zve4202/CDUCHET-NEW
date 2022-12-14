using DevExpress.XtraEditors;
using GH.XlShablon;
using System.Collections.Generic;
using static GH.Windows.DevExpressHelper;

namespace Tester.forms
{
    public partial class ExcelDbProcSetting : XtraUserControl, IExtMenuControl
    {

        public ExcelDbProcSetting()
        {
            InitializeComponent();
            Reset();
            SetupLookups(this);
        }

        public int TestType => comboType.SelectedIndex;

        public int stock_id => comboStock.SelectedIndex;
        public int client_id => (int)(comboClients.EditValue ?? 0);

        private Dictionary<int, string> _clients;
        public Dictionary<int, string> Clients
        {
            get => _clients;
            set
            {
                if (_clients != null)
                    _clients = null;

                _clients = value;
                comboClients.Properties.DataSource = _clients;
                if (_clients != null)
                {
                    _clients.Add(0, "Все реализаторы");
                    comboClients.EditValue = 0;
                }
            }
        }

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
