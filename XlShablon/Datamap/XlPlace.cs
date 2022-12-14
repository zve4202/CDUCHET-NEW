using DevExpress.Utils;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GH.XlShablon
{


    internal class XlPlace : Control
    {
        FieldParam _owner;
        bool DragPresent { get => _owner.DragPresent; set => _owner.DragPresent = value; }
        ExcelPanel ExcelPanel
        {
            get => _owner.ExcelPanel;
            set
            {
                _owner.ExcelPanel = value;
                toolTip.Active = (value != null);
            }
        }
        bool Required { get => _owner.Required; /*set => _owner.Required = value;*/ }

        new public Color BackColor
        {
            get { return DragPresent ? Color.LightYellow : Required ? Color.LightPink : Color.LightGray; }
        }

        new public Color ForeColor
        {
            get { return Required ? Color.DarkRed : Color.Gray; }
        }

        ToolTip toolTip { get; }

        public XlPlace(FieldParam owner)
        {
            _owner = owner;
            Dock = DockStyle.Fill;
            AutoSize = false;
            Width = 80;
            AllowDrop = true;
            toolTip = new ToolTip();
            toolTip.Active = false;
            toolTip.ShowAlways = true;
            toolTip.SetToolTip(this, "Дважды кликните, чтобы очистить полк...");
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);           

            
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            ExcelPanel = null;
            base.OnDoubleClick(e);
            Invalidate();
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            drgevent.Effect = DragDropEffects.Copy;
            base.OnDragOver(drgevent);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            ExcelPanel = (ExcelPanel)Drag.Dragged;
            DragPresent = false;
            Refresh();
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            DragPresent = true;
            base.OnDragEnter(drgevent);
            Refresh();
        }

        protected override void OnDragLeave(EventArgs e)
        {
            DragPresent = false;
            base.OnDragLeave(e);
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            if (!DragPresent && ExcelPanel != null)
            {
                e.Graphics.FillRectangle(new SolidBrush(ExcelPanel.BackColor), ClientRectangle);
                int x = (Height - ExcelPanel.Height) / 2;
                Rectangle xlRect = new Rectangle(0, 0, ExcelPanel.Width, ExcelPanel.Height);

                xlRect.Offset(0, x);
                e.Graphics.DrawImage(CursorUtil.AsBitmap(ExcelPanel), xlRect);
            }
            else
            {
                e.Graphics.Clear(BackColor);
                e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
                Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(new Pen(ForeColor, 1), rect);

            }

            base.OnPaint(e);
        }
    }


}
