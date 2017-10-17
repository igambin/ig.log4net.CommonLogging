using System;
using System.Threading.Tasks;

namespace ig.log4net.Caching
{
    public interface ICachable : IDisposable
    {
        T GetOrSet<T>(string key, Func<T> dataGetter, DateTimeOffset absoluteExpirationOffset);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> dataGetter, DateTimeOffset absoluteExpirationOffset);
        T GetOrSet<T>(string key, Func<T> dataGetter, TimeSpan slidingExpirationOffset);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> dataGetter, TimeSpan slidingExpirationOffset);
        T GetOrSet<T>(string key, Func<T> dataGetter);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> dataGetter);
    }
}