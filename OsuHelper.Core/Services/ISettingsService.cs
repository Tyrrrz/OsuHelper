namespace OsuHelper.Services
{
    public interface ISettingsService
    {
        string OsuApiRoot { get; set; }
        string OsuApiKey { get; set; }

        void Load();
        void Save();
    }
}