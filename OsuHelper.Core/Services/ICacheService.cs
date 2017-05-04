namespace OsuHelper.Services
{
    public interface ICacheService
    {
        void Store<T>(string key, T obj);

        T RetrieveOrDefault<T>(string key, T defaultValue = default(T));
    }
}