using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ig.log4net.Logging.Appenders.MailRecipientManagement;
using ig.log4net.Logging.LogModels;
using log4net;

namespace ig.log4net.Logging
{
    public static class GenericLoggingExtensions
    {
        public static ILogger Logger<TClass>(this TClass klass, Recipients recipients = Recipients.Default, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            where TClass : class => CreateLogger(LogManager.GetLogger<TClass>(), recipients, file, member, line);

        public static ILogger FileLogger<TClass>(this TClass klass, Recipients recipients = Recipients.Default, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            where TClass : class => CreateLogger(LogManager.GetLogger("FileLogger"), recipients, file, member, line);

        public static ILogger MailLogger<TClass>(this TClass klass, Recipients recipients = Recipients.Default, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
            where TClass : class => CreateLogger(LogManager.GetLogger("MailLogger"), recipients, file, member, line, true);

        private static ILogger CreateLogger(ILogger logger, Recipients recipients, string file, string member, int line, bool sendMails = false)
        {
            ThreadContext.Properties["caller"] = $"[{file}:{line}({member})]";
            ThreadContext.Properties["recipients"] = recipients;
            if (sendMails)
            {
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            }
            if (Settings.SettingsReader.Logging.EvaluateStackTraces)
            {
                ThreadContext.Properties["stacktrace"] = Environment.StackTrace;
            }
            return logger;
        }

        private static void ProcessIfEnabled(bool isEnabled, Action action)
        {
            if (isEnabled) action();
        }

        public static void DebugDump<TEntity>(this ILogger logger, ISerializingLogItem<TEntity> logItem, Exception ex = null, params string[] errors)
            => ProcessIfEnabled(logger.IsDebugEnabled, () => PrepareMessageAndLogToBlobStorage(logItem, ex, errors, logger.Debug));

        public static void DebugFormat<TEntity>(this ILogger logger, string format, TEntity entity, params Func<TEntity, object>[] accessorFuncs)
            => ProcessIfEnabled(logger.IsDebugEnabled, () => logger.DebugFormat($"{typeof(TEntity).Name}::({format})", accessorFuncs.Select(x => x(entity)).ToArray()));

        public static void InfoDump<TEntity>(this ILogger logger, ISerializingLogItem<TEntity> logItem, Exception ex = null, params string[] errors)
            => ProcessIfEnabled(logger.IsInfoEnabled, () => PrepareMessageAndLogToBlobStorage(logItem, ex, errors, logger.Info));

        public static void InfoFormat<TEntity>(this ILogger logger, string format, TEntity entity, params Func<TEntity, object>[] accessorFuncs)
            => ProcessIfEnabled(logger.IsInfoEnabled, () => logger.InfoFormat($"{typeof(TEntity).Name}::({format})", accessorFuncs.Select(x => x(entity)).ToArray()));

        public static void WarnDump<TEntity>(this ILogger logger, ISerializingLogItem<TEntity> logItem, Exception ex = null, params string[] errors)
            => ProcessIfEnabled(logger.IsWarnEnabled, () => PrepareMessageAndLogToBlobStorage(logItem, ex, errors, logger.Warn));

        public static void WarnFormat<TEntity>(this ILogger logger, string format, TEntity entity, params Func<TEntity, object>[] accessorFuncs)
            => ProcessIfEnabled(logger.IsWarnEnabled, () => logger.WarnFormat($"{typeof(TEntity).Name}::({format})", accessorFuncs.Select(x => x(entity)).ToArray()));

        public static void ErrorDump<TEntity>(this ILogger logger, ISerializingLogItem<TEntity> logItem, Exception ex = null, params string[] errors)
            => ProcessIfEnabled(logger.IsErrorEnabled, () => PrepareMessageAndLogToBlobStorage(logItem, ex, errors, logger.Error));

        public static void ErrorFormat<TEntity>(this ILogger logger, string format, TEntity entity, params Func<TEntity, object>[] accessorFuncs)
            => ProcessIfEnabled(logger.IsErrorEnabled, () => logger.ErrorFormat($"{typeof(TEntity).Name}::({format})", accessorFuncs.Select(x => x(entity)).ToArray()));

        public static void FatalDump<TEntity>(this ILogger logger, ISerializingLogItem<TEntity> logItem, Exception ex = null, params string[] errors)
            => ProcessIfEnabled(logger.IsFatalEnabled, () => PrepareMessageAndLogToBlobStorage(logItem, ex, errors, logger.Fatal));

        public static void FatalFormat<TEntity>(this ILogger logger, string format, TEntity entity, params Func<TEntity, object>[] accessorFuncs)
            => ProcessIfEnabled(logger.IsFatalEnabled, () => logger.FatalFormat($"{typeof(TEntity).Name}::({format})", accessorFuncs.Select(x => x(entity)).ToArray()));


        private static void PrepareMessageAndLogToBlobStorage<TEntity>(ISerializingLogItem<TEntity> logItem, Exception ex,
            IEnumerable<string> errors, Action<string, Exception> logAction)
        {
            logItem.Errors = errors;
            logItem.Exception = ex;

            if (logItem.IsNotifyEMailRecipients)
            {
                ThreadContext.Properties["notifyEMailRecipients"] = "1";
            }

            var message = logItem.SimpleLogMessage;

            ThreadContext.Properties["logItem"] = logItem;

            logAction(message, ex);
        }

    }
}
