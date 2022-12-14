using DevExpress.Utils;
using DevExpress.XtraEditors;
using GH.Windows;
using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GH.XlShablon
{
    public class FieldParam : PanelControl
    {
        public enum ParamFunctionType { Usual, IsKey, ToSumm, OutSourceData, Combiner }

        public FieldParam(XlShablon shablon, string caption, string name, Type paramType, int length, bool required = false)
        {
            Shablon = shablon;


            Required = required;

            caption = (caption.Trim() + ":").Replace("::", ":");

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
            Label.Width = 120;
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

        public string Caption(int index = -1)
        {
            if (index == -1)
                return Label.Text.Replace(":", "");
            return Label.Text.Replace(":", " ") + index.ToString();
        }

        XlShablon Shablon;

        LabelControl _label;
        XlPlace _place;

        protected FieldsMap FieldsMap => Shablon.DataMap;

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

                Shablon.RefreshControlsState();
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
        public ParamFunctionType ParamFunc { get; set; }

        internal void SetColumnValue(DataRow row)
        {
            row[Column] = ExcelValue(row);
        }

        public object ColumnValue(DataRow row)
        {
            return ConvertValue(row[Column]);
        }

        public object ExcelValue(DataRow row)
        {
            if (ExcelPanel == null)
                return null;

            return ConvertValue(row[ExcelPanel.Column]);

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

                    string text = Regex.Replace(val.ToString(), @"\D", "");

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

        private void ShablonResize(object sender, System.EventArgs e)
        {
            Label.Width = (Width) / 3;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
            {
                Parent.Resize += ShablonResize;
            }
            base.OnParentChanged(e);
        }


        internal async void Blink(string text)
        {
            string savetext = Place.Text;
            Place.Text = text;
            await Task.Factory.StartNew(() =>
            {
                int time = 0;
                while (time < 10)
                {
                    Place.Blink = (time % 2 == 0);
                    Place.InvokeIfRequired(() =>
                    {
                        Place.Refresh();
                    });
                    Application.DoEvents();
                    Thread.Sleep(75);
                    time++;
                }
            });
            Place.Blink = false;
            Place.Text = savetext;
        }


        internal virtual bool Validate(bool showDlg = true)
        {
            if (Required && ExcelPanel == null)
            {
                if (showDlg)
                {
                    Blink("Заполните это поле!!!");
                }
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

        public void RemoveSourceColumn()
        {
            if (Column != ExcelPanel.Column)
            {
                ExcelPanel.Column.Table.Columns.Remove(ExcelPanel.Column);
            }
        }
    }


}
