using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GH.XlShablon
{
    public struct IconInfo
    {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    public class CursorUtil
    {
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr handle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        // Based on the article and comments here:
        // http://www.switchonthecode.com/tutorials/csharp-tutorial-how-to-use-custom-cursors
        // Note that the returned Cursor must be disposed of after use, or you'll leak memory!
        public static Cursor CreateCursor(Bitmap bm, int xHotspot, int yHotspot)
        {
            IntPtr cursorPtr;
            IntPtr ptr = bm.GetHicon();
            IconInfo tmp = new IconInfo();
            GetIconInfo(ptr, ref tmp);
            tmp.xHotspot = xHotspot;
            tmp.yHotspot = yHotspot;
            tmp.fIcon = false;
            cursorPtr = CreateIconIndirect(ref tmp);

            if (tmp.hbmColor != IntPtr.Zero) DeleteObject(tmp.hbmColor);
            if (tmp.hbmMask != IntPtr.Zero) DeleteObject(tmp.hbmMask);
            if (ptr != IntPtr.Zero) DestroyIcon(ptr);

            return new Cursor(cursorPtr);
        }

        public static Bitmap AsBitmap(Control c)
        {
            Bitmap bm = new Bitmap(c.Width, c.Height);
            c.DrawToBitmap(bm, new Rectangle(0, 0, c.Width, c.Height));
            return bm;
        }

        public static Bitmap AddCopySymbol(Bitmap bm)
        {
            return (Bitmap)bm.Clone();
        }

        public static Bitmap AddNoSymbol(Bitmap bm)
        {
            return (Bitmap)bm.Clone();
        }
    }

    public class Drag
    {
        public static Point DragStart { get; set; }
        public static Control Dragged { get; private set; }
        public static Cursor DragCursorMove { get; private set; }
        public static Cursor DragCursorLink { get; private set; }
        public static Cursor DragCursorCopy { get; private set; }
        public static Cursor DragCursorNo { get; private set; }

        public static void StartDragging(Control c)
        {
            Dragged = c;
            DisposeOldCursors();
            Bitmap bm = CursorUtil.AsBitmap(c);
            DragCursorMove = CursorUtil.CreateCursor((Bitmap)bm.Clone(), DragStart.X, DragStart.Y);
            DragCursorLink = CursorUtil.CreateCursor((Bitmap)bm.Clone(), DragStart.X, DragStart.Y);
            DragCursorCopy = CursorUtil.CreateCursor(CursorUtil.AddCopySymbol(bm), DragStart.X, DragStart.Y);
            DragCursorNo = CursorUtil.CreateCursor(CursorUtil.AddNoSymbol(bm), DragStart.X, DragStart.Y);
            //Debug.WriteLine("Starting drag");
        }

        private static void DisposeOldCursors()
        {
            if (DragCursorMove != null)
            {
                CursorUtil.DeleteObject(DragCursorMove.Handle);
                DragCursorMove = null;
            }
            if (DragCursorLink != null)
            {
                CursorUtil.DeleteObject(DragCursorLink.Handle);
                DragCursorLink = null;
            }
            if (DragCursorCopy != null)
            {
                CursorUtil.DeleteObject(DragCursorCopy.Handle);
                DragCursorCopy = null;
            }
            if (DragCursorNo != null)
            {
                CursorUtil.DeleteObject(DragCursorNo.Handle);
                DragCursorNo = null;
            }
        }

        // This gets called once when we move over a new control,
        // or continuously if that control supports dropping.
        public static void UpdateCursor(object sender, GiveFeedbackEventArgs e)
        {
            //Debug.WriteLine(MainForm.MousePosition);
            e.UseDefaultCursors = false;
            //Debug.WriteLine("effect = " + e.Effect);

            if (e.Effect == DragDropEffects.Move)
            {
                Cursor.Current = DragCursorMove;

            }
            else if (e.Effect == DragDropEffects.Copy)
            {
                Cursor.Current = DragCursorCopy;

            }
            else if (e.Effect == DragDropEffects.None)
            {
                Cursor.Current = DragCursorNo;

            }
            else if (e.Effect == DragDropEffects.Link)
            {
                Cursor.Current = DragCursorLink;

            }
            else
            {
                Cursor.Current = DragCursorMove;
            }
        }
    }


}
