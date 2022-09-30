using DevExpress.Utils;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GH.XlShablon
{
    internal class ExcelMap : List<ExcelPanel>
    {
        new public void Clear()
        {
            foreach (var item in ToArray())
                item.Dispose();

            base.Clear();
        }
    }

    public class ExcelPanel : LabelControl
    {
        bool mousePresent = false;

        private int _col;
        //public int Col { get => _col; }

        private DataColumn _column;
        public DataColumn Column { get => _column; set => _column = value; }

        private int _places;
        public bool Mapped => _places > 0;

        public Type DataType => _column.DataType;

        public override string Text
        {
            get
            {
                if (_column == null)
                    return base.Text;
                return _column.Caption + ": " + base.Text/* ?? ""*/;
            }
            set { base.Text = value; }
        }

        public string ColText
        {
            get
            {
                return ((char)(_col + 65)).ToString();
            }
        }


        public ExcelPanel(int i, DataColumn column)
        {
            _col = i;
            _column = column;

            Padding = new Padding(2);
            AutoSizeMode = LabelAutoSizeMode.Vertical;
            //BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            Dock = DockStyle.Top;


            Appearance.BackColor = Color.White;
            Appearance.ForeColor = Color.Black;
            Appearance.TextOptions.VAlignment = VertAlignment.Center;
            Appearance.TextOptions.WordWrap = WordWrap.NoWrap;
            Appearance.TextOptions.Trimming = Trimming.EllipsisCharacter;

            Appearance.Image = GetImage();
            Appearance.Options.UseBackColor = true;
            Appearance.Options.UseImage = true;
            ImageAlignToText = ImageAlignToText.LeftCenter;
            IndentBetweenImageAndText = 5;


            MouseDown += ExcelPanel_MouseDown;
            GiveFeedback += new GiveFeedbackEventHandler(Drag.UpdateCursor);
            MouseEnter += ExcelPanel_MouseEnter;
            MouseLeave += ExcelPanel_MouseLeave;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (mousePresent)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Gray, 1), rect);
            }
        }

        private void ExcelPanel_MouseLeave(object sender, EventArgs e)
        {
            mousePresent = false;
        }

        private void ExcelPanel_MouseEnter(object sender, EventArgs e)
        {
            mousePresent = true;
        }

        private void ExcelPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            Select();
            Drag.DragStart = new Point(e.X, e.Y);
            Drag.StartDragging(this);

            //Bitmap bmp = new Bitmap(Width, Height);
            //DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
            ////optionally define a transparent color
            //bmp.MakeTransparent(Color.Magenta);

            //Cursor cur = new Cursor(bmp.GetHicon());
            //Cursor.Current = cur;

            DoDragDrop(this, DragDropEffects.Copy);
        }

        internal void DecPlaces()
        {
            _places--;
            if (_places == 0)
            {
                Appearance.BackColor = Color.White;
            }
        }

        internal void IncPlaces()
        {
            _places++;
            if (_places == 1)
            {
                Appearance.BackColor = Color.LightYellow;
            }
        }

        Image GetImage()
        {
            Bitmap bitmap = new Bitmap(32, 16, PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width - 1, bitmap.Height - 1);
            using (Font font = new Font("Courier New", 10/*, FontStyle.Bold*/))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.FillRectangle(Brushes.LightGray, rect);
                    g.DrawRectangle(new Pen(Color.Gray, 1), rect);
                    var size = g.MeasureString(ColText, font);
                    g.DrawString(ColText, font, Brushes.Black, GetTextPosition(bitmap, size)/*, StringFormat.GenericTypographic*/);
                }
            }
            return bitmap;
        }

        private PointF GetTextPosition(Image image, SizeF size)
        {
            PointF point = new PointF((image.Width - size.Width) / 2,
                (image.Height - size.Height) / 2);
            return point;
        }

    }

}
