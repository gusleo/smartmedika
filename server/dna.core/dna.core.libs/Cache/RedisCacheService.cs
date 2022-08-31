﻿using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dna.core.libs.Cache
{
    public class RedisCacheService : ICacheService
    {
        protected IDistributedCache Cache;
        private static int DefaultCacheDuration => 60;

        public RedisCacheService(IDistributedCache cache)
        {
            Cache = cache;
        }

        public void Store(string key, object content)
        {
            Store(key, content, DefaultCacheDuration);
        }

        public void Store(string key, object content, int duration)
        {
            string toStore;
            if ( content is string )
            {
                toStore = (string)content;
            }
            else
            {
                toStore = JsonConvert.SerializeObject(content);
            }

            duration = duration <= 0 ? DefaultCacheDuration : duration;
            Cache.Set(key, Encoding.UTF8.GetBytes(toStore), new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.UtcNow + TimeSpan.FromSeconds(duration)
            });
        }

        public T Get<T>(string key) where T : class
        {
            var fromCache = Cache.Get(key);
            if ( fromCache == null )
            {
                return null;
            }

            var str = Encoding.UTF8.GetString(fromCache);
            if ( typeof(T) == typeof(string) )
            {
                return str as T;
            }

            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
