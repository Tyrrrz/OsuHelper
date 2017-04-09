using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OsuHelper.Services
{
    public class HttpService : IHttpService, IDisposable
    {
        private const int MaxActiveConnections = 15;
        private static readonly TimeSpan ThrottlingDelay = TimeSpan.FromMilliseconds(200);

        private readonly HttpClient _client;
        private int _activeConnections;

        public HttpService()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            handler.UseCookies = false;

            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("User-Agent", "osu!helper (github.com/Tyrrrz/OsuHelper)");
            _client.DefaultRequestHeaders.Add("Connection", "Close");
        }

        ~HttpService()
        {
            Dispose(false);
        }

        public async Task<string> GetStringAsync(string url)
        {
            while (_activeConnections >= MaxActiveConnections)
                await Task.Delay(ThrottlingDelay);

            _activeConnections++;
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