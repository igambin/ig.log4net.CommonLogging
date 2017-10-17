using System;
using System.Collections.Generic;
using System.Linq;

namespace ig.log4net.Extensions
{
    public static class ExceptionExtensions
    {
        public static string MessageDelimiter { get; } = " > ";

        public static string MessageListItem { get; } = "> ";

        public static int MessageIndentationWidth { get; } = 2;

        public static IList<string> MessageList(this Exception ex, IList<string> messages = null)
        {
            if (messages == null) messages = new List<string>();
            messages.Add(ex.Message);
            if (ex.InnerException != null)
                return ex.InnerException.MessageList(messages);
            return messages;
        }

        public static IList<string> MessageList(this AggregateException ex)
        {
            var msgs = new List<string>();
            foreach (var e in ex.InnerExceptions)
            {
                msgs.AddRange(e.MessageList());
            }
            return msgs;
        }

        private static string ToIndent(this int indent) => indent > 0 ? new string(' ', indent * MessageIndentationWidth) : string.Empty;

        public static string Messages(this Exception ex, int indents = 0, bool includeStrackTrace = true)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            var msg = indents.ToIndent().Append(MessageListItem)
                             .AppendLine(ex.Message)
                             .Append((indents + 1).ToIndent());
            if (includeStrackTrace)
            {
                msg = msg.AppendLine(ex.StackTrace?.Replace(Environment.NewLine,
                                         $"{Environment.NewLine}{(indents + 1).ToIndent()}") ?? "");
            }
            msg = msg.AppendLine();

            if (ex.InnerException != null)
            {
                return msg + ex.InnerException.Messages(indents + 1, includeStrackTrace);
            }
            return msg;
        }

        public static string Messages(this AggregateException ex, bool includeStrackTrace = true)
        {
            string joinedMessage = string.Empty;

            if (ex?.InnerExceptions?.Any() ?? false)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    joinedMessage = joinedMessage.AppendLine(e.Messages(includeStrackTrace: includeStrackTrace));
                }
            }

            return joinedMessage;
        }

        public static bool HasInnerExceptionOfType<T>(this Exception ex) where T : Exception
        {
            if (ex.GetType() == typeof(T))
            {
                return true;
            }
            if (ex.InnerException != null)
            {
                return ex.InnerException.HasInnerExceptionOfType<T>();
            }
            return false;
        }
    }
}
