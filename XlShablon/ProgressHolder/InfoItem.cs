using DevExpress.XtraEditors;
using GH.Utils;

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
            caption.Text = info.GetAttributeValue<IinfoAttribute, string>(x => x.Description);
            Visible = false;
            ClientSize = new System.Drawing.Size(panelInfo.Width, panelInfo.Height);
        }

        new public string Text { get => message.Text; set => message.Text = value; }
    }
}
