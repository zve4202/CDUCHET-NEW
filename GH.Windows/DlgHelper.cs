using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GH.Windows
{
    public static class DlgHelper
    {
        private static Form mainForm;
        public static Form MainForm
        {
            get
            {
                if (mainForm == null && Application.OpenForms.Count > 0)
                    mainForm = Application.OpenForms[0];
                return mainForm;
            }
        }

        public static DialogResult DialogResult { get; private set; }

        private static void MessageBox(object param)
        {
            XtraMessageBoxArgs args = param as XtraMessageBoxArgs;

            if (MainForm == null)
            {
                if (MainForm.InvokeRequired)
                {
                    MainForm.Invoke((Action)delegate
                    {
                        DialogResult = XtraMessageBox.Show(args);
                    });
                }
                else
                    DialogResult = XtraMessageBox.Show(args);
            }
        }

        private static XtraMessageBoxArgs GetArgs(string message, Icon icon, bool yesNo = false)
        {
            XtraMessageBoxArgs args = new XtraMessageBoxArgs();
            if (MainForm == null)
                args.Caption = Application.ProductName;
            else
                args.Caption = MainForm.Text;
            args.Icon = icon;
            args.Text = message;
            if (yesNo)
            {
                args.Buttons = new DialogResult[2];
                args.Buttons[0] = DialogResult.Yes;
                args.Buttons[1] = DialogResult.No;
                args.DefaultButtonIndex = 1;
            }
            else
            {
                args.Buttons = new DialogResult[1];
                args.Buttons[0] = DialogResult.OK;
                args.DefaultButtonIndex = 0;
            }
            args.Owner = MainForm;
            return args;
        }

        public static bool DlgYesNo(string message)
        {
            XtraMessageBoxArgs args = GetArgs(message, SystemIcons.Question, true);
            MessageBox(args);
            return DialogResult == DialogResult.Yes;
        }

        public static void DlgInfo(string message)
        {
            VoidDlg(GetArgs(message, SystemIcons.Information));
        }

        public static void DlgWarning(string message)
        {
            VoidDlg(GetArgs(message, SystemIcons.Warning));
        }

        public static void DlgError(string message)
        {
            VoidDlg(GetArgs(message, SystemIcons.Error));
        }

        private static void VoidDlg(XtraMessageBoxArgs args)
        {
            MessageBox(args);
        }
    }

}
