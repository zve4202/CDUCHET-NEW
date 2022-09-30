using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using GH.Configs;
using GH.XlShablon;
using System.Windows.Forms;

namespace Tester
{
    public partial class mainForm : XtraForm, IMainForm
    {
        public mainForm()
        {
            InitializeComponent();
            SetControls(this);
        }

        void SetControls(Control control)
        {
            if (!(control is XtraTabControl))
                control.Padding = new Padding(5);

            foreach (var item in control.Controls)
            {
                if (item is XlShablon)
                {
                    continue;
                }
                else if (item is Control ctrl)
                {
                    if (ctrl.Controls.Count > 0)
                        SetControls(ctrl);
                }
            }
        }
    }
}
