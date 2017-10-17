using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ig.log4net.Extensions;
using ig.log4net.Logging.Appenders.MailRecipientManagement;
using ig.log4net.Logging.LogModels;
using ig.log4net.Settings;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace ig.log4net.Logging.Appenders
{
    public partial class MailNotificationAppender : AppenderSkeleton
    {
        public int DefaultSmtpPort { get; } = 25;
        public int MaxSubjectLength { get; set; }
        public int MaxBodyLengthForLogging { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpLogin { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpSender { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var logItem = ThreadContext.Properties["logItem"] as LogItem;

            var recipientGroup = ThreadContext.Properties["recipients"] as Recipients? ?? Recipients.Default;

            var subject = EvaluateSubject(logItem, RenderLoggingEvent(loggingEvent));

            subject = subject.Length > MaxSubjectLength ? subject.Substring(0, MaxSubjectLength - 3) + "..." : subject;

            var recipients = CreateRecipients(recipientGroup);

            string body = GetBody(logItem, loggingEvent);

            SendMail(subject, body, recipients);
        }

        private string GetBody(LogItem logItem, LoggingEvent loggingEvent)
        {
            var body = logItem == null ? RenderLoggingEvent(loggingEvent) : LogItemBody(logItem);
            if (ThreadContext.Properties["stacktrace"] is string stacktrace)
                body = $"{body}{Environment.NewLine}{Environment.NewLine}Log-CallStack : {stacktrace}";
            return body;
        }

        private string LogItemBody(LogItem logItem)
        {
            if (logItem == null) throw new ArgumentNullException(nameof(logItem), @"To render a LogItem, it has to be passed as an argument!");
            return  $"Type name     : {logItem.DataItemType}{Environment.NewLine}" +
                    $"Message       : {Environment.NewLine}{Environment.NewLine}{logItem.MailLogMessage}";
        }

        private string EvaluateSubject(LogItem logitem = null, string fallbackSubject = "")
        {
            var env = SettingsReader.ApplicationSettings.EnvKey;
            var machineName = SettingsReader.ApplicationSettings.MachineName;
            var mailsubject = logitem?.MailSubject ?? fallbackSubject;
            return $"EAI ({env}) {mailsubject} [{machineName}]";
        }

        public class MailRecipient
        {
            public string ToAddr { get; set; }
            public string FromAddr { get; set; }
            public string Subject { get; set; }
        }

        private List<MailRecipient> CreateRecipients(Recipients recipientGroup)
        {
            var recipients = recipientGroup.GetRecipients();

            var addresses = recipients.SelectMany(
                        x => x.RecipientList
                            .Where(r => !string.IsNullOrWhiteSpace(r))
                            .Select(y => new MailRecipient { ToAddr = y, Subject = x.PrefixSubject("", MaxSubjectLength), FromAddr = SmtpSender }))
                    .ToList();

            return addresses.Distinct(new MailRecipientEqualityComparer()).ToList();
        }

        private void SendMail(string subject, string body, List<MailRecipient> recipients)
        {
            var time = DateTime.Now.Ticks;
            var resultCollection = new ConcurrentBag<string>();

            var bodypart = body.Length > MaxBodyLengthForLogging ? body.Substring(0, MaxBodyLengthForLogging - 3) + "..." : body;
            var logMessage = $"Mail Id {time}" + Environment.NewLine +
                             $"Send mail to {string.Join(", ", recipients.Select(x => x.ToAddr))}" + Environment.NewLine +
                             $"concerning {subject}" + Environment.NewLine +
                             $"containing {bodypart}" + Environment.NewLine + Environment.NewLine;

            Parallel.ForEach(recipients,
                recipient =>
                {
                    try
                    {
                        var mail = new MailMessage(recipient.FromAddr, recipient.ToAddr)
                        {
                            Subject = recipient.Subject,
                            Body = body,
                            BodyEncoding = Encoding.UTF8,
                            DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                        };

                        var client = new SmtpClient
                        {
                            Port = SmtpPort.GetTOrDefault(DefaultSmtpPort),
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(SmtpLogin, SmtpPassword),
                            Host = SmtpHost
                        };

                        client.Send(mail);

                        var succMsg = $"Sent Email '{recipient.Subject}' from '{recipient.FromAddr}' to '{recipient.ToAddr}' successfully.";
                        resultCollection.Add(succMsg);
                    }
                    catch (Exception ex)
                    {
                        var failMsg = $"Sending Email '{recipient.Subject}' from '{recipient.FromAddr}' to '{recipient.ToAddr}' failed: {ex.Messages()}";
                        resultCollection.Add(failMsg);
                    }
                }
            );
            this.Logger().Debug(logMessage + string.Join(Environment.NewLine, resultCollection));
        }


    }
}
