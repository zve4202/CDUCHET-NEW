using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GH.Windows
{
    public static class WindowHelper
    {
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);


        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static async void WaitAndKeysendWindowAsinc(string win_title, string sent_text)
        {
            int wait_step = 0;
            while (wait_step < 1000)
            {
                await Task.Delay(50);
                IntPtr DialogHandle = FindWindow(null, win_title);
                if (DialogHandle != IntPtr.Zero)
                {
                    SetForegroundWindow(DialogHandle);
                    if (sent_text == "")
                        SendKeys.Send("{ENTER}");
                    else
                    {
                        await Task.Delay(100);
                        SendKeys.Send(sent_text + "{ENTER}");
                    }
                    break;
                }
                wait_step++;
            }
        }

        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static void ControlExecute(MethodInvoker action, string processName)
        {
            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    foreach (Process proc in processes)
                    {
                        try
                        {
                            proc.Kill();
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
