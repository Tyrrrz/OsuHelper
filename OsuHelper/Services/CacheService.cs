using System;
using System.IO;
using Newtonsoft.Json;

namespace OsuHelper.Services
{
    public class CacheService
    {
        private readonly string _cacheDirPath;

        public CacheService()
        {
            _cacheDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache\\");
        }

        private string GetCacheFilePath(string id)
        {
            Directory.CreateDirectory(_cacheDirPath);
            return Path.Combine(_cacheDirPath, id + ".ch");
        }

        public void Store(string key, object obj)
        {
            if (obj is string objStr)
            {
                Directory.CreateDirectory(_cacheDirPath);
                File.WriteAllText(GetCacheFilePath(key), objStr);
            }
            else if (obj is Stream objStream)
            {
                Directory.CreateDirectory(_cacheDirPath);
                using (var output = File.Create(GetCacheFilePath(key)))
                    objStream.CopyTo(output);
            }
            else
            {
                var serialized = JsonConvert.SerializeObject(obj);
                Directory.CreateDirectory(_cacheDirPath);
                File.WriteAllText(GetCacheFilePath(key), serialized);
            }
        }

        public T RetrieveOrDefault<T>(string key, T defaultValue = default(T))
        {
            if (typeof(T) == typeof(string))
            {
                if (File.Exists(GetCacheFilePath(key)))
                    return (T) (object) File.ReadAllText(GetCacheFilePath(key));

                return defaultValue;
            }
            if (typeof(T) == typeof(Stream))
            {
                if (File.Exists(GetCacheFilePath(key)))
                    return (T) (object) File.OpenRead(GetCacheFilePath(key));

                return defaultValue;
            }
            else
            {
                if (File.Exists(GetCacheFilePath(key)))
                {
                    var serialized = File.ReadAllText(GetCacheFilePath(key));
                    return JsonConvert.DeserializeObject<T>(serialized);
                }

                return defaultValue;
            }
        }
    }
}