namespace OsuHelper.Services
{
    public interface ICacheService
    {
        void Store(string key, object obj);

        T RetrieveOrDefault<T>(string key, T defaultValue = default(T));
    }
}