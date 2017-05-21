using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OsuHelper.Services
{
    public class HttpService : IHttpService, IDisposable
    {
        private const int MaxConcurrentRequests = 15;

        private readonly HttpClient _client;
        private readonly SemaphoreSlim _semaphoreSlim;

        public HttpService()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            handler.UseCookies = false;

            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("User-Agent", "osu!helper (github.com/Tyrrrz/OsuHelper)");
            _client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");

            _semaphoreSlim = new SemaphoreSlim(MaxConcurrentRequests);
        }

        ~HttpService()
        {
            Dispose(false);
        }

        public async Task<string> GetStringAsync(string url)
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                return await _client.GetStringAsync(url);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public async Task<Stream> GetStreamAsync(string url)
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                return await _client.GetStreamAsync(url);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client.Dispose();
                _semaphoreSlim.Dispose();
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