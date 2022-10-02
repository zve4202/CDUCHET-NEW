using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using GH.Utils;
using GH.Windows;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Tester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");

            AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
            {
                Logger.Error(e.ExceptionObject);
                DlgHelper.DlgError(e.ExceptionObject.ToString());
            };


            Application.ThreadException += delegate (object sender, ThreadExceptionEventArgs e)
            {
                Logger.Error(e.Exception);
                DlgHelper.DlgError(e.Exception.ToString());
                Environment.Exit(0);
            };
            Application.Run(new mainForm());
        }
    }
}
