using System;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;

namespace OsuHelper.Services
{
    public class AudioService : IAudioService, IDisposable
    {
        private readonly WaveOut _player;

        private TaskCompletionSource<object> _playbackTcs;

        public bool IsPlaying => _player.PlaybackState == PlaybackState.Playing;

        public AudioService()
        {
            _player = new WaveOut();
            _player.PlaybackStopped += (sender, args) => _playbackTcs?.TrySetResult(null);
        }

        public async Task PlayAsync(Stream stream)
        {
            await StopAsync();

            using (var reader = new Mp3FileReader(stream))
            {
                _playbackTcs = new TaskCompletionSource<object>();
                _player.Init(reader);
                _player.Play();
                await _playbackTcs.Task;
            }
        }

        public async Task StopAsync()
        {
            if (_playbackTcs == null) return;

            _player.Stop();
            await _playbackTcs.Task;
        }

        public void Dispose()
        {
            _player.Dispose();
        }
    }
}