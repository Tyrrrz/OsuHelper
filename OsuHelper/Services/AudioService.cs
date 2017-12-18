using System;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;

namespace OsuHelper.Services
{
    public class AudioService : IAudioService, IDisposable
    {
        private readonly WaveOut _player;

        private TaskCompletionSource<object> _tcs;

        public bool IsPlaying => _player.PlaybackState == PlaybackState.Playing;

        public AudioService()
        {
            _player = new WaveOut();
            _player.PlaybackStopped += (sender, args) =>
            {
                if (_tcs == null) return;
                _tcs.TrySetResult(null);
            };
        }

        ~AudioService()
        {
            Dispose(false);
        }

        public async Task PlayAsync(Stream stream)
        {
            using (var reader = new Mp3FileReader(stream))
            {
                _tcs = new TaskCompletionSource<object>();
                _player.Init(reader);
                _player.Play();
                await _tcs.Task;
            }
        }

        public async Task StopAsync()
        {
            if (_tcs == null || _tcs.Task.IsCompleted) return;
            _player.Stop();
            await _tcs.Task;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _player.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}