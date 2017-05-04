using System;
using System.IO;
using Newtonsoft.Json;

namespace OsuHelper.Services
{
    public class FileCacheService : ICacheService
    {
        private readonly string _cacheDirPath;

        public FileCacheService()
        {
            _cacheDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache\\");
        }

        private string GetCacheFilePath(string id)
        {
            return Path.Combine(_cacheDirPath, id + ".ch");
        }

        public void Store<T>(string key, T obj)
        {
            string id = typeof(T).Name + "_" + key;

            string serialized = JsonConvert.SerializeObject(obj);
            Directory.CreateDirectory(_cacheDirPath);
            File.WriteAllText(GetCacheFilePath(id), serialized);
        }

        public T RetrieveOrDefault<T>(string key, T defaultValue = default(T))
        {
            string id = typeof(T).Name + "_" + key;

            if (File.Exists(GetCacheFilePath(id)))
            {
                string serialized = File.ReadAllText(GetCacheFilePath(id));
                return JsonConvert.DeserializeObject<T>(serialized);
            }

            return defaultValue;
        }
    }
}