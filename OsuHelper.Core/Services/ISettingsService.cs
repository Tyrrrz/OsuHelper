namespace OsuHelper.Services
{
    public interface ISettingsService
    {
        string DataSourceApiRoot { get; set; }
        string DataSourceApiKey { get; set; }

        void Load();
        void Save();
    }
}