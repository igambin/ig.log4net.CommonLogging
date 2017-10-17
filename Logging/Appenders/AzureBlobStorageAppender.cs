using System;
using System.Text;
using ig.log4net.Extensions;
using ig.log4net.Logging.LogModels;
using log4net;
using log4net.Appender;
using log4net.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ig.log4net.Logging.Appenders
{
    public class AzureBlobStorageAppender : AppenderSkeleton
    {
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _blobContainer;

        public string AzureStorageConnectionString { get; set; }
        public string BlobContainerReferenceName { get; set; } 

        public override void ActivateOptions()
        {
            try
            {
                _storageAccount = CloudStorageAccount.Parse(AzureStorageConnectionString);
                _blobClient = _storageAccount.CreateCloudBlobClient();
                _blobContainer = _blobClient.GetContainerReference(BlobContainerReferenceName);
                _blobContainer.CreateIfNotExists();

                base.ActivateOptions();
            }
            catch (Exception ex)
            {
                this.FileLogger().Warn("Configuring AzureStorage failed!", ex);
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (_blobContainer == null) return;
                
            var logItem = ThreadContext.Properties["logItem"] as LogItem;
            try
            {
                var d = DateTime.Now;

                string filename = "", message = "";
                if (logItem != null)
                {
                    filename = logItem.BlobName ?? $"AzureBlobStorageAppender/{d:yyyyMMddHHmmss.ffffff}_MissingFilename.log";
                    message = logItem.BlobLogMessage ?? "[null]";
                    if (ThreadContext.Properties["caller"] is string caller)
                        message = caller.AppendLine(message);
                }
                else
                {
                    filename = $"{loggingEvent.LoggerName}/{d:yyyy}/{d:MM}/{d:dd}/{loggingEvent.Level.Name}.log";
                    message = RenderLoggingEvent(loggingEvent);
                }


                var blockref = _blobContainer.GetAppendBlobReference(filename);
                blockref.CreateOrReplace(AccessCondition.GenerateIfNotExistsCondition());

                blockref.AppendText(message, Encoding.UTF8);

                if (logItem?.PayloadStream != null)
                    blockref.AppendFromByteArray(logItem.PayloadStream, 0, logItem.PayloadStream.Length);
                
                if (ThreadContext.Properties["stacktrace"] is string stacktrace)
                    blockref.AppendText("".AppendNewLine(2).AppendLine($"Log-CallStack : {stacktrace}"));
               
            }
            catch (Exception e)
            {
                this.FileLogger().Warn("Appending Log to AzureStorage failed!", e);
            }
        }
    }
}
