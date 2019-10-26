using System;
using NLog;

namespace MPP3.config
{
    public static class NLogConfiguration
    {
        private static void Сonfigure()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt" };
            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            
            LogManager.Configuration = config;
        }

        public static Logger GetLogger(string name)
        {
            Сonfigure();
            return LogManager.GetLogger(name);
        }
    }
}