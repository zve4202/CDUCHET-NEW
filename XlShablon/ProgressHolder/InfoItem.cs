using DevExpress.XtraEditors;
using GH.Utils;
using System;

namespace GH.XlShablon
{
    public partial class InfoItem : XtraUserControl
    {
        public InfoItem()
        {
            InitializeComponent();
        }

        public InfoItem(InfoNames info)
        {
            InitializeComponent();
            Visible = false;
            caption.Text = info.GetAttributeValue<IinfoAttribute, string>(x => x.Description) + ":";
            message.Text = "";
            ClientSize = new System.Drawing.Size(panelInfo.Width, panelInfo.Height);
        }

        new public string Text { get => message.Text; set => message.Text = value; }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                panelInfo_Resize(this, System.EventArgs.Empty);

            base.OnParentChanged(e);
        }

        private void panelInfo_Resize(object sender, System.EventArgs e)
        {
            caption.Width = (panelInfo.ClientSize.Width + panelInfo.Padding.Left) / 3;
        }
    }
}
