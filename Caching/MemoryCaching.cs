using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace ig.log4net.Caching
{
    public class MemoryCaching : ICachable
    {
        public T GetOrSet<T>(string key, Func<T> dataGetter, DateTimeOffset absoluteExpirationOffset)
        {
            CachedItem<T> data = MemoryCache.Default.Get(key) as CachedItem<T>;
            if (data == null)
            {
                data = new CachedItem<T> { Item = dataGetter() };

                var policy = new CacheItemPolicy { AbsoluteExpiration = absoluteExpirationOffset };

                MemoryCache.Default.Add(key, data, policy);
            }
            return data.Item;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> dataGetter, DateTimeOffset absoluteExpirationOffset)
        {
            CachedItem<T> data = MemoryCache.Default.Get(key) as CachedItem<T>;
            if (data == null)
            {
                data = new CachedItem<T>() {Item = await dataGetter()};

                var policy = new CacheItemPolicy { AbsoluteExpiration = absoluteExpirationOffset };

                MemoryCache.Default.Add(key, data, policy);
            }
            return data.Item;
        }

        public T GetOrSet<T>(string key, Func<T> dataGetter, TimeSpan slidingExpirationInterval)
        {
            CachedItem<T> data = MemoryCache.Default.Get(key) as CachedItem<T>;
            if (data == null)
            {
                data = new CachedItem<T>() { Item = dataGetter() };

                var policy = new CacheItemPolicy { SlidingExpiration = slidingExpirationInterval };

                MemoryCache.Default.Add(key, data, policy);
            }
            return data.Item;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> dataGetter, TimeSpan slidingExpirationInterval)
        {
            CachedItem<T> data = MemoryCache.Default.Get(key) as CachedItem<T>;
            if (data == null)
            {
                data = new CachedItem<T>() { Item = await dataGetter() };

                var policy = new CacheItemPolicy { SlidingExpiration = slidingExpirationInterval };

                MemoryCache.Default.Add(key, data, policy);
            }
            return data.Item;
        }

        public T GetOrSet<T>(string key, Func<T> dataGetter) => GetOrSet(key, dataGetter, TimeSpan.FromMinutes(30d));

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> dataGetter)
            => GetOrSetAsync(key, dataGetter, TimeSpan.FromMinutes(30d));



        #region implement IDisposable to satisfy CodeAnalysis Evaluation for CA1063
        // Dispose() calls Dispose(true)  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // NOTE: Leave out the finalizer altogether if this class doesn't   
        // own unmanaged resources itself, but leave the other methods  
        // exactly as they are.   
        ~MemoryCaching()
        {
            // Finalizer calls Dispose(false)  
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)  
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources  
                //if (managedResource != null)
                //{
                //    managedResource.Dispose();
                //    managedResource = null;
                //}
            }
            // free native resources if there are any.  
            //if (nativeResource != IntPtr.Zero)
            //{
            //    Marshal.FreeHGlobal(nativeResource);
            //    nativeResource = IntPtr.Zero;
            //}
        }
        #endregion
    }
}
