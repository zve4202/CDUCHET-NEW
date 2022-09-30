using System.Diagnostics;
using System.IO;

namespace GH.Utils
{
    public static class AppHelper
    {
        public const string AppKeyName = "Application.key";
        public const string ExportFolder = "Exports";
        public const string ConfigsFolder = "Configs";
        public const string LogsFolder = "Logs";
        public const string DownloadWebFolder = "Downloads";
        public const string ImportFolder = "Imports";
        public const string OrdersFolder = "Orders";
        public const string DatabaseFolder = "DB";
        public const string ProcessedFolder = "Processed";

        private static readonly Process _appProcess = Process.GetCurrentProcess();
        private static string _exePath;
        public static string ExePath
        {
            get
            {
                if (string.IsNullOrEmpty(_exePath))
                    _exePath = _appProcess.MainModule.FileName;
                return _exePath;
            }
        }

        private static string _startupPath;
        public static string StartupPath
        {
            get
            {
                if (string.IsNullOrEmpty(_startupPath))
                    _startupPath = Path.GetDirectoryName(ExePath);
                return _startupPath;
            }
        }
    }
}
