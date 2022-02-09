using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Externals.Caching
{
    public interface ICacheManager
    {
        void Set<T>(string cacheKey, T cacheItem) where T : class;
        T Get<T>(string cacheKey) where T : class;
    }
}
