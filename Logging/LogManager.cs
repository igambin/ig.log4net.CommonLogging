using System;
using System.IO;
using ig.log4net.Extensions;

namespace ig.log4net.Logging
{
    public class LogManager : ILogManager
    {
        private static ILogManager Instance { get; }
        public MailNotificationLevel MailNotificationLevel { get; set; }
        public bool AlwaysNotifyByMailOnException { get; set; }

        static LogManager()
        {
            var configFile = new FileInfo(Path.Combine(typeof(LogManager).AssemblyDirectory(), "log4net.config"));
            global::log4net.Config.XmlConfigurator.ConfigureAndWatch(configFile);
            Instance = new LogManager();
            
        }
        
        public static ILogger GetLogger<T>() 
            => Instance.GetLogger(typeof(T));

        public ILogger GetLogger(Type type) 
            => new LoggerAdapter(global::log4net.LogManager.GetLogger(type));

        public static ILogger GetLogger(string name)
            => new LoggerAdapter(global::log4net.LogManager.GetLogger(name));
    }
}
