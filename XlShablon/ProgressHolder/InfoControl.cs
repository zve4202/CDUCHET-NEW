using DevExpress.XtraEditors;
using GH.Windows;
using System;
using System.Windows.Forms;

namespace GH.XlShablon
{

    public partial class InfoControl : XtraUserControl
    {

        public BindingSource DataSource => _dataSource;

        public InfoControl()
        {
            InitializeComponent();
        }

        public InfoControl(ProgressHolder holder)
        {
            InitializeComponent();

            DataSource.DataSource = holder;


            foreach (InfoNames info in (InfoNames[])Enum.GetValues(typeof(InfoNames)))
            {
                InfoItem infoItem = new InfoItem(info);
                panelInfo.Controls.Add(infoItem);
                infoItem.Visible = false;
                infoItem.Dock = DockStyle.Top;
                infoItem.SendToBack();
                holder.AddDictInfo(info, infoItem);
            }

            panelInfo.AutoSize = true;
            panelInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ProgressMode = false;
        }

        public bool ProgressMode
        {
            get => panelInfo.Visible;
            set
            {
                this.InvokeIfRequired(() =>
                {
                    panelInfo.Visible = value;
                    labelInfo.Visible = !panelInfo.Visible;
                });
            }
        }

    }
}
