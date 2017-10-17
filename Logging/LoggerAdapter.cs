using System;
using System.IO;
using System.Linq;
using ig.log4net.Extensions;
using ig.log4net.Logging.Serializers.Xml2CSharp;
using log4net;
using ig.log4net.XmlTools;

namespace ig.log4net.Logging
{
    public class LoggerAdapter : ILogger
    {
        private readonly ILog _log;
        public MailNotificationLevel MailNotificationLevel { get; }
        public bool AlwaysNotifyByMailOnException { get; }

        internal LoggerAdapter(ILog log)
        {
            var configFileName = Path.Combine(typeof(LoggerAdapter).AssemblyDirectory(), "log4net.config");
            var xml = File.ReadAllText(configFileName);
            var log4NetParams = Serialization.Deserialize<Log4NetConfig>(xml).Param;
            Console.WriteLine("params read from Log4NetConfig: " + string.Join(" ", log4NetParams.Select(x => $@"{{{x.Name}=>{x.Value}}}")));
            _log = log;
            MailNotificationLevel = log4NetParams.FirstOrDefault(x => x.Name == nameof(MailNotificationLevel))?.Value.GetTOrDefault(MailNotificationLevel.Error) ?? MailNotificationLevel.Error;
            AlwaysNotifyByMailOnException = log4NetParams.FirstOrDefault(x => x.Name == nameof(AlwaysNotifyByMailOnException))?.Value.GetTOrDefault(true) ?? true;
        }

        public bool IsDebugEnabled => _log.IsDebugEnabled;

        public bool IsInfoEnabled => _log.IsInfoEnabled;

        public bool IsWarnEnabled => _log.IsWarnEnabled;

        public bool IsErrorEnabled => _log.IsErrorEnabled;

        public bool IsFatalEnabled => _log.IsFatalEnabled;

        public void Debug(object message)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Debug)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            if ((MailNotificationLevel <= MailNotificationLevel.Debug)
               || (AlwaysNotifyByMailOnException && exception != null)
               )
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Debug)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.DebugFormat(format, args);
        }

        public void DebugFormat(string format, object arg0)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Debug)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Debug)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Debug)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.DebugFormat(format, arg0, arg1, arg2);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Debug)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.DebugFormat(provider, format, args);
        }

        public void Info(object message)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Info)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            if (exception != null)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Info)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.InfoFormat(format, args);
        }

        public void InfoFormat(string format, object arg0)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Info)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.InfoFormat(format, arg0);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Info)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Info)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Info)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.InfoFormat(provider, format, args);
        }

        public void Warn(object message)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Warn)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            if ((MailNotificationLevel <= MailNotificationLevel.Warn)
                || (AlwaysNotifyByMailOnException && exception != null)
            )
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Warn)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.WarnFormat(format, args);
        }

        public void WarnFormat(string format, object arg0)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Warn)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Warn)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Warn)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Warn)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.WarnFormat(provider, format, args);
        }

        public void Error(object message)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Error)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            if ((MailNotificationLevel <= MailNotificationLevel.Error)
                || (AlwaysNotifyByMailOnException && exception != null)
            )
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Error)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.ErrorFormat(format, args);
        }

        public void ErrorFormat(string format, object arg0)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Error)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Error)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Error)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Error)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.ErrorFormat(provider, format, args);
        }

        public void Fatal(object message)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Fatal)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            if ((MailNotificationLevel <= MailNotificationLevel.Fatal)
                || (AlwaysNotifyByMailOnException && exception != null)
            )
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Fatal)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.FatalFormat(format, args);
        }

        public void FatalFormat(string format, object arg0)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Fatal)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.FatalFormat(format, arg0);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Fatal)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Fatal)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.FatalFormat(format, arg0, arg1, arg2);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (MailNotificationLevel <= MailNotificationLevel.Fatal)
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            _log.FatalFormat(provider, format, args);
        }

    }
}
