using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OsuHelper.Services
{
    public class HttpService : IHttpService, IDisposable
    {
        private readonly HttpClient _client;

        public HttpService()
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

        public async Task<string> GetStringAsync(string url)
        {
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