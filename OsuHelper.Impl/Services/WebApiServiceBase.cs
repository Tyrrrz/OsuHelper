using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OsuHelper.Services
{
    public abstract class WebApiServiceBase : IDisposable
    {
        private readonly HttpClient _client;

        private readonly TimeSpan _minRequestInterval = TimeSpan.FromSeconds(0.05);
        private DateTime _lastRequestDateTime = DateTime.MinValue;

        protected WebApiServiceBase()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "osu!helper (github.com/Tyrrrz/OsuHelper)");
        }

        ~WebApiServiceBase()
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

        protected async Task<string> GetStringAsync(string url)
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