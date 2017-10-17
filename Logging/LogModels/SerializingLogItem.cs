using System;
using System.Linq;
using ig.log4net.Extensions;
using ig.log4net.Logging.Serializers;

namespace ig.log4net.Logging.LogModels
{
    [Serializable]
    public class SerializingLogItem<TItem> : LogItem, ISerializingLogItem<TItem>
    {
        protected static IDataSerializer<TItem> DefaultSerializer { get; } = new JsonDataSerializer<TItem>();

        public SerializingLogItem(TItem dataItem, SerializerType serializer = SerializerType.Json, Exception exception = null, params string[] errorMessages)
        {
            LogItemCreated = DateTimeOffset.Now;
            TrackingId = $"{Guid.NewGuid()}";
            DataItem = dataItem;
            SimpleLogMessage = $"Logging {DataItemType}-Item";
            if (exception != null) Exception = exception;
            if (errorMessages.Any()) Errors = errorMessages;
            SetSerializer(serializer);
            _blobMessage = null;
            _mailMessage = null;
        }

        public DateTimeOffset LogItemCreated { get; }

        public TItem DataItem
        {
            get => (TItem) DataObject;
            set
            {
                DataObject = value;
                RenderPayloadAsStream();
            }
        }

        #region serializer-related
        protected IDataSerializer<TItem> Serializer { get; set; }

        protected string FileEnding { get; set; }

        protected void SetSerializer(SerializerType serializerType)
        {
            if (serializerType == SerializerType.None)
            {
                if (Serializer == null)
                {
                    Serializer = DefaultSerializer;
                }
            }
            else
            {
                Serializer = serializerType.GetSerializer<TItem>();
            }
            FileEnding = serializerType.FileEnding();
        }
        #endregion
        public override string BlobName => $"{TrackingId}_{DataItemType}_{LogItemCreated}.{FileEnding}";

        protected void RenderBlob(SerializerType serializerType)
        {
            SetSerializer(serializerType);
            RenderBlob();
            RenderPayloadAsStream();
        }

        protected override void RenderBlob()
        {
            SerializeData();
        }

        public override string TrackingId { get; }

        protected void RenderPayloadAsStream()
        {
            PayloadStream = Serializer?.Serialize(DataItem);
        }

        protected void SerializeData()
        {
            _mailMessage = _blobMessage = "";
            AddOriginToMessage();
            AddExceptionToMessage();
            AddErrorsToMessage();
            if (!IsRenderPayloadOnly)
            {
                AddToMessage("Payload:".AppendLine(), true);
            }
        }

        private string _blobMessage;

        public override string SimpleLogMessage { get; }

        public override string BlobLogMessage
        {
            get
            {
                if (_blobMessage == null)
                {
                    SerializeData();
                }
                return _blobMessage;
            }
            set => _blobMessage = value;
        }

        private string _mailMessage;
        public override string MailLogMessage
        {
            get
            {
                if (_mailMessage==null)
                {
                    SerializeData();
                }
                return _mailMessage;
            }
            set => _mailMessage = value;
        }
    }
}