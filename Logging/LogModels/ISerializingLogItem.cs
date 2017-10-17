namespace ig.log4net.Logging.LogModels
{
    public interface ISerializingLogItem<TItem> : ILogItem
    {
        TItem DataItem { get; set; }

    }
}