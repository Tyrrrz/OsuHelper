namespace OsuHelper.Services
{
    public interface ICacheService
    {
        void Store<T>(string key, T obj) where T : class;

        T RetrieveOrDefault<T>(string key, T defaultValue = default(T)) where T : class;
    }
}