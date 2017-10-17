using System;
using System.Collections.Generic;
using System.Linq;
using ig.log4net.Extensions;

namespace ig.log4net.Logging.LogModels
{
    [Serializable]
    public abstract class LogItem : ILogItem
    {
        public static string InitialLogFileDirectory => "";

        protected LogItem()
        {
            Errors = new List<string>();
        }

        protected object DataObject { get; set; }

        public Type DataItemType => DataObject?.GetType();

        protected abstract void RenderBlob();

        public abstract string TrackingId { get; }

        public abstract string BlobName { get; }

        #region BlobLogMessage
        public abstract string SimpleLogMessage { get; }
        public abstract string BlobLogMessage { get; set; }
        public abstract string MailLogMessage { get; set; }

        public string MailSubject => $"{(string.IsNullOrWhiteSpace(TrackingId) || TrackingId == "noTrackingId" ? "noTrackingId" : $"TID:{TrackingId}")} ".Trim();

        public byte[] PayloadStream { get; set; }
        protected virtual bool IsRenderPayloadOnly { get; } = false;
        public bool IsNotifyEMailRecipients => Exception != null || Errors.Any();
        #endregion

        #region Exception
        public Exception Exception { get; set; }

        protected virtual void AddExceptionToMessage()
        {
            AddToMessage("".AppendLine("Exception(s):")
                           .AppendLine()
                           .AppendLine(Exception.Messages())
                           .AppendNewLine(),
                            !IsRenderPayloadOnly && Exception != null,
                            Exception != null);
        }
        #endregion

        protected void AddOriginToMessage()
        {
            AddToMessage(""
                .AppendLine($"   Machine : {Settings.SettingsReader.ApplicationSettings.MachineName}")
                .AppendLine($"  Executed : {DateTime.Now:O}"),
                !IsRenderPayloadOnly , true);
        }

        #region ErrorMessages
        public IEnumerable<string> Errors { get; set; }

        public virtual string RenderErrors => string.Join("", Errors.Select(e => e.AppendLine()));

        protected virtual void AddErrorsToMessage()
        {
            AddToMessage(
                "".AppendLine("Errors:").AppendLine(RenderErrors).AppendLine(),
                !IsRenderPayloadOnly && Errors.Any(),
                Errors.Any());
        }
        #endregion

        protected void AddToMessage(string toAdd, bool renderForBlobLog = false, bool renderForMailLog = false)
        {
            toAdd = toAdd ?? string.Empty;
            if (renderForBlobLog)
            {
                BlobLogMessage = BlobLogMessage.Append(toAdd);
            }
            if (renderForMailLog)
            {
                MailLogMessage = MailLogMessage.Append(toAdd);
            }
        }
    }
}

