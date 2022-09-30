using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Diagnostics;
using System.IO;

namespace GH.Utils
{
    public static class Logger
    {
        private static readonly Process _currentProcess = Process.GetCurrentProcess();


        private static readonly bool _isConfigurated = Setup();

        private static ILog _log = null;
        private static ILog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger(_currentProcess.ProcessName);
                }
                return _log;
            }
        }

        enum RollerType
        {
            Info,
            Error,
        }

        public static bool Setup()
        {
            if (!_isConfigurated)
            {

                Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
                ConfigRoller(RollerType.Error, hierarchy);


                hierarchy.Root.Level = Level.Info;
                hierarchy.Configured = true;
                return true;
            }
            else
                return _isConfigurated;
        }

        private static void ConfigRoller(RollerType rollerType, Hierarchy hierarchy)
        {
            PatternLayout patternLayout = new PatternLayout();
            switch (rollerType)
            {
                case RollerType.Info:
                    patternLayout.ConversionPattern = "%date{yyyy-dd-MM HH:mm:ss}  %-5p %logger - %m%n";
                    break;
                case RollerType.Error:
                    patternLayout.ConversionPattern = "%date{yyyy-dd-MM HH:mm:ss} [%thread] %-5level %logger - %message%newline";
                    break;
                default:
                    break;
            }
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();

            LevelRangeFilter filter = new LevelRangeFilter();
            switch (rollerType)
            {
                case RollerType.Info:
                    filter.LevelMin = Level.Verbose;
                    filter.LevelMax = Level.Info;
                    break;
                case RollerType.Error:
                    filter.LevelMin = Level.Error;
                    filter.LevelMax = Level.Fatal;
                    break;
                default:
                    break;
            }
            filter.ActivateOptions();

            roller.AddFilter(filter);
            roller.AppendToFile = true;

            switch (rollerType)
            {
                case RollerType.Info:
                    roller.File = Path.Combine("Logs", "Events.log");
                    break;
                case RollerType.Error:
                    roller.File = Path.Combine("Logs", "Errors.log");
                    break;
                default:
                    break;
            }
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "5MB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();

            hierarchy.Root.AddAppender(roller);
        }

        public static void Error(object message)
        {
            if (Skip(message))
                return;
            Log.Error(message);
        }

        private static bool Skip(object message)
        {
            return message is Exception ex && ex.InnerException is UserWantExitException;
        }

        public static void Error(object message, Exception exception)
        {
            if (Skip(message))
                return;
            Log.Error(message, exception);
        }

        public static void ErrorFormatted(string format, params object[] args)
        {
            Log.ErrorFormat(format, args);
        }

        public static void Fatal(object message)
        {
            if (Skip(message))
                return;
            Log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            if (Skip(message))
                return;
            Log.Fatal(message, exception);
        }

        public static void FatalFormatted(string format, params object[] args)
        {
            Log.FatalFormat(format, args);
        }
    }

}
