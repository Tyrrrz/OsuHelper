using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OsuHelper.Services
{
    public class HttpService : IHttpService, IDisposable
    {
        private const int MaxActiveConnections = 40;
        private static readonly TimeSpan ThrottlingDelay = TimeSpan.FromMilliseconds(50);

        private readonly HttpClient _client;
        private DateTime _lastRequestTime;
        private int _activeConnections;

        public HttpService()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            handler.UseCookies = false;

            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("User-Agent", "osu!helper (github.com/Tyrrrz/OsuHelper)");
            _client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
        }

        ~HttpService()
        {
            Dispose(false);
        }

        private async Task ThrottleRequests()
        {
            // Frequency-based throttling
            var timeLeft = ThrottlingDelay - (DateTime.Now - _lastRequestTime);
            if (timeLeft > TimeSpan.Zero)
                await Task.Delay(timeLeft);

            // Pressure-based throttling
            while (_activeConnections >= MaxActiveConnections)
                await Task.Delay(ThrottlingDelay);
        }

        public async Task<string> GetStringAsync(string url)
        {
            await ThrottleRequests();

            _activeConnections++;
            _lastRequestTime = DateTime.Now;
            var result = await _client.GetStringAsync(url);
            _activeConnections--;
            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}