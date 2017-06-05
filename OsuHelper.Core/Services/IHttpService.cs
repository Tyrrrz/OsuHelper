using System.IO;
using System.Threading.Tasks;

namespace OsuHelper.Services
{
    public interface IHttpService
    {
        Task<string> GetStringAsync(string url);

        Task<Stream> GetStreamAsync(string url);
    }
}