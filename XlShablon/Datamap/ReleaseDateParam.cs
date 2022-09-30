using DevExpress.Utils;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace GH.XlShablon
{
    internal class ReleaseDateParam : FieldParam
    {
        LabelControl fmtLabel;
        private string _format;
        public string Format
        {
            get => _format;
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = null;
                _format = value;
                if (fmtLabel != null)
                    fmtLabel.Text = value == null ? "???" : value;
            }
        }

        public override ExcelPanel ExcelPanel
        {
            get => base.ExcelPanel;
            set
            {
                if (base.ExcelPanel == value)
                    return;

                base.ExcelPanel = value;

                if (base.ExcelPanel != null)
                {
                    if (fmtLabel != null)
                    {
                        fmtLabel.Visible = DataType != base.ExcelPanel.DataType;
                    }
                }
            }
        }

        public ReleaseDateParam(string caption, string name) : base(caption, name, typeof(DateTime), 0, false)
        {
            fmtLabel = new LabelControl();
            fmtLabel.Appearance.BackColor = Color.White;
            fmtLabel.Appearance.ForeColor = Color.Black;
            fmtLabel.Appearance.TextOptions.VAlignment = VertAlignment.Center;
            fmtLabel.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            fmtLabel.Appearance.TextOptions.WordWrap = WordWrap.NoWrap;
            fmtLabel.Appearance.TextOptions.Trimming = Trimming.EllipsisCharacter;
            fmtLabel.Appearance.Options.UseBackColor = true;
            fmtLabel.Dock = DockStyle.Right;
            fmtLabel.AutoSizeMode = LabelAutoSizeMode.Horizontal;
            fmtLabel.Width = 50;
            fmtLabel.Padding = new Padding(2);
            fmtLabel.Parent = this;
            fmtLabel.Text = "???";
            fmtLabel.ShowToolTips = true;
            fmtLabel.ToolTip = "Кликните, чтобы ввести маску данных...";
            fmtLabel.Click += FmtLabel_Click;
            fmtLabel.BringToFront();
            fmtLabel.Visible = false;
        }

        private void FmtLabel_Click(object sender, EventArgs e)
        {
            if (ExcelPanel == null)
            {
                Format = null;
                return;
            }

            using (FormatForm form = new FormatForm(ExcelPanel.Column, Format))
            {
                if (form.ShowDialog() == DialogResult.OK)
                    Format = form.formatEdit.Text;
            }
        }

        public override object ConvertValue(object val)
        {
            if (val != null)
            {
                if (val.GetType() == typeof(DateTime))
                    return val;

                if (val.GetType() == typeof(double))
                    return (DateTime)val;

                if (val.GetType() == typeof(string))
                    return GetDateValue(val);
            }
            return DBNull.Value;
        }

        private object GetDateValue(object val)
        {
            try
            {
                DateTime date = DateTime.ParseExact(val.ToString(), Format, CultureInfo.GetCultureInfo("de-DE"));
                return date;
            }
            catch { }
            return DBNull.Value;
        }

        internal override bool Validate(bool showDlg = true)
        {
            if (base.Validate(showDlg) && fmtLabel != null && Format == null)
            {
                if (showDlg)
                {

                    Utils.DlgHelper.DlgError($"Не заполнена маска данных у {Label.Text}.\r\n" +
                        "Кликните в \"???\" чтоб залонить маску.");
                    return false;
                }
            }

            return true;
        }




        internal override void Clear()
        {
            Format = null;
            base.Clear();
        }



    }


}
