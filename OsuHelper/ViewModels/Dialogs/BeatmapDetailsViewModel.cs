using System.Diagnostics;
using System.Net;
using OsuHelper.Exceptions;
using OsuHelper.Models;
using OsuHelper.Services;
using OsuHelper.ViewModels.Framework;

namespace OsuHelper.ViewModels.Dialogs
{
    public class BeatmapDetailsViewModel : DialogScreen
    {
        private readonly SettingsService _settingsService;
        private readonly DataService _dataService;
        private readonly AudioService _audioService;

        public Beatmap Beatmap { get; set; }

        public bool IsPreviewPlaying { get; private set; }

        public BeatmapDetailsViewModel(SettingsService settingsService, DataService dataService,
            AudioService audioService)
        {
            _settingsService = settingsService;
            _dataService = dataService;
            _audioService = audioService;
        }

        public void OpenPage()
        {
            Process.Start($"https://osu.ppy.sh/beatmaps/{Beatmap.Id}");
        }

        public void Download()
        {
            var url = $"https://osu.ppy.sh/beatmapsets/{Beatmap.MapSetId}/download";

            // If configured to download without video - append a parameter to the request
            if (_settingsService.DownloadWithoutVideo)
                url += "?noVideo=1";

            Process.Start(url);
        }

        public void DownloadDirect()
        {
            Process.Start($"osu://dl/{Beatmap.MapSetId}");
        }

        public void DownloadBloodcat()
        {
            Process.Start($"http://bloodcat.com/osu/s/{Beatmap.MapSetId}");
        }

        public bool CanPlayPreview => !IsPreviewPlaying;

        public async void PlayPreview()
        {
            IsPreviewPlaying = true;

            try
            {
                using (var stream = await _dataService.GetBeatmapSetPreviewAsync(Beatmap.MapSetId))
                    await _audioService.PlayAsync(stream);
            }
            catch (HttpErrorStatusCodeException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
            {
                // Preview not available
            }

            IsPreviewPlaying = false;
        }

        public bool CanStopPreview => IsPreviewPlaying;

        public async void StopPreview()
        {
            await _audioService.StopAsync();
            IsPreviewPlaying = false;
        }

        public void TogglePreview()
        {
            if (IsPreviewPlaying)
                StopPreview();
            else
                PlayPreview();
        }
    }
}