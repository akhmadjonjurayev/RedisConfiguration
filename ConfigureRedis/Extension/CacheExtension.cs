using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureRedis.Extension
{
    public static class CacheExtension
    {
        public static T GetData<T>(this IDistributedCache cache, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    var data = cache.Get(key);
                    return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
                }
                catch { }
            }
            throw new System.Exception("error-not-found-data");
        }

        public static async Task<T> GetDataAsync<T>(this IDistributedCache cache, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    var data = await cache.GetAsync(key);
                    return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
                }
                catch { }
            }
            throw new System.Exception("error-not-found-data");
        }

        public static void SetData<T>(this IDistributedCache cache, string key, T value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var data = JsonConvert.SerializeObject(value);
                cache.SetData(key, Encoding.UTF8.GetBytes(data));
            }
        }

        public static async Task SetDataAsync<T>(this IDistributedCache cache, string key, T value)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var data = JsonConvert.SerializeObject(value);
                    await cache.SetDataAsync(key, data);
                }
            }
            catch { throw; }
        }
    }
}
