using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Externals.Caching
{
    public class CacheManager : ICacheManager
    {
        public static MemoryCache GLobalCache = MemoryCache.Default;
        public T Get<T>(string cacheKey) where T : class
        {
            T objData = default(T);
            try
            {
                objData = (T)GLobalCache.Get(cacheKey);
            }
            catch (Exception ex)
            {

            }
            return objData;
        }

        public void Set<T>(string cacheKey, T cacheItem) where T : class
        {
            try
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.Priority = System.Runtime.Caching.CacheItemPriority.NotRemovable;
                policy.AbsoluteExpiration = System.DateTime.Now.AddMinutes(30);
                policy.SlidingExpiration = MemoryCache.NoSlidingExpiration;
                GLobalCache.Set(cacheKey, cacheItem, policy);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
