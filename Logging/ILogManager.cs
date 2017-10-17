using System;

namespace ig.log4net.Logging
{
    public interface ILogManager
    {
        ILogger GetLogger(Type type);
    }
}