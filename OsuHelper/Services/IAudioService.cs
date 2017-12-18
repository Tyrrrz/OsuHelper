using System.IO;
using System.Threading.Tasks;

namespace OsuHelper.Services
{
    public interface IAudioService
    {
        bool IsPlaying { get; }

        Task PlayAsync(Stream stream);

        Task StopAsync();
    }
}