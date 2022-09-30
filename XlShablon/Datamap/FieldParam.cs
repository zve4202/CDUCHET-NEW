using DevExpress.Utils;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace GH.XlShablon
{
    internal class FieldParam : PanelControl
    {
        protected FieldsMap FieldsMap => FieldsMap.Instance;
        LabelControl _label;
        XlPlace _place;

        private ExcelPanel _excelPanel;

        public virtual ExcelPanel ExcelPanel
        {
            get => _excelPanel;
            set
            {
                if (_excelPanel == value)
                    return;

                if (_excelPanel != null)
                {
                    _excelPanel.DecPlaces();
                }

                _excelPanel = value;

                if (_excelPanel != null)
                {
                    _excelPanel.IncPlaces();
                }

                XlShablon.Shablon.EnableButtons();
            }
        }


        public bool NeedToFill
        {
            get
            {
                if (ExcelPanel == null || Column == null)
                    return false;

                if (Column.ColumnName != ExcelPanel.Column.ColumnName)
                    return true;

                if (Column.DataType == ExcelPanel.Column.DataType)
                    return false;

                return true;
            }
        }
        public DataColumn Column { get; internal set; }
        public bool Required { get; set; }
        public Type DataType { get; set; }
        public int Length { get; set; } = 0;

        internal void SetColumnValue(DataRow row)
        {
            row[Column] = ConvertValue(ExcelValue(row));
        }

        public object ExcelValue(DataRow row)
        {
            if (ExcelPanel == null)
                return null;

            return row[ExcelPanel.Column];

        }

        public virtual object ConvertValue(object val)
        {
            if (val != null)
            {               

                if (DataType == typeof(string))
                    return val.ToString().PadLeft(Length).Trim();

                if (DataType == typeof(int))
                {
                    if (val.GetType() == typeof(int))
                        return val;

                    string text = "";
                    foreach (var c in val.ToString())
                    {
                        if (char.IsDigit(c))
                            text = string.Join(text, c);
                        else
                            break;
                    }

                    if (text == "")
                        return DBNull.Value;

                    if (int.TryParse(text, out int ret))
                        return ret;

                    return DBNull.Value;

                }

                if (DataType == typeof(DateTime))
                {

                    if (val.GetType() == typeof(DateTime))
                        return val;

                    if (val.GetType() == typeof(double))
                        return (DateTime)val;
                }
            }
            return DBNull.Value;

        }

        public bool DragPresent { get; set; }
        internal XlPlace Place { get => _place; set => _place = value; }
        public LabelControl Label { get => _label; set => _label = value; }

        public FieldParam(string caption, string name, Type paramType, int length, bool required = false)
        {
            Required = required;

            Label = new LabelControl();
            Label.Appearance.BackColor = Color.White;
            Label.Appearance.ForeColor = Color.Black;
            Label.Appearance.TextOptions.VAlignment = VertAlignment.Center;
            Label.Appearance.TextOptions.HAlignment = HorzAlignment.Far;
            Label.Appearance.TextOptions.WordWrap = WordWrap.NoWrap;
            Label.Appearance.TextOptions.Trimming = Trimming.EllipsisCharacter;
            Label.Appearance.Options.UseBackColor = true;
            Label.Dock = DockStyle.Left;
            Label.AutoSizeMode = LabelAutoSizeMode.None;
            Label.Width = 80;
            Label.Padding = new Padding(2);
            Label.Parent = this;
            Label.Text = caption;


            

            Place = new XlPlace(this);
            Place.Parent = this;
            Place.BringToFront();

            //AllowDrop = true;

            Name = name;

            DataType = paramType;
            if (length < 0)
                length = 0;
            if (paramType == typeof(string) && length == 0)
                length = 255;

            Length = length;

            Height = 26;
            Padding = new Padding(2);
            Dock = DockStyle.Top;

        }

        internal virtual bool Validate(bool showDlg = true)
        {
            if (Required && ExcelPanel == null)
            {
                if (showDlg)
                    Utils.DlgHelper.DlgError($"Заполните поле {Label.Text}");
                return false;

            }

            return true;
        }
        

        internal virtual void Clear()
        {
            ExcelPanel = null;
            DragPresent = false;
            Place.Refresh();
        }
    }


}
