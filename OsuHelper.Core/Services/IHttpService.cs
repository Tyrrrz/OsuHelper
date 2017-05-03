using System.Threading.Tasks;

namespace OsuHelper.Services
{
    public interface IHttpService
    {
        Task<string> GetStringAsync(string url);

        Task DownloadAsync(string url, string filePath);
    }
}