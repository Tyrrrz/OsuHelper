using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OsuHelper.Services
{
    public class HttpService : IHttpService, IDisposable
    {
        private readonly HttpClient _client;

        private readonly TimeSpan _minRequestInterval = TimeSpan.FromSeconds(0.05);
        private DateTime _lastRequestDateTime = DateTime.MinValue;

        protected HttpService()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("User-Agent", "osu!helper (github.com/Tyrrrz/OsuHelper)");
        }

        ~HttpService()
        {
            Dispose(false);
        }

        private async Task RequestThrottlingAsync()
        {
            var timeSinceLastRequest = DateTime.Now - _lastRequestDateTime;
            if (timeSinceLastRequest > TimeSpan.Zero && timeSinceLastRequest < _minRequestInterval)
            {
                var timeLeft = _minRequestInterval - timeSinceLastRequest;
                await Task.Delay(timeLeft);
            }
            _lastRequestDateTime = DateTime.Now;
        }

        public async Task<string> GetStringAsync(string url)
        {
            await RequestThrottlingAsync();
            return await _client.GetStringAsync(url);
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