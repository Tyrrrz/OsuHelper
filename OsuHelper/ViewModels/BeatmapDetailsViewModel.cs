using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OsuHelper.Messages;
using OsuHelper.Models;
using OsuHelper.Services;

namespace OsuHelper.ViewModels
{
    public class BeatmapDetailsViewModel : ViewModelBase, IBeatmapDetailsViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IDataService _dataService;
        private readonly IAudioService _audioService;

        private Beatmap _beatmap;
        private bool _isPreviewPlaying;

        public Beatmap Beatmap
        {
            get => _beatmap;
            private set => Set(ref _beatmap, value);
        }

        public bool IsPreviewPlaying
        {
            get => _isPreviewPlaying;
            private set
            {
                Set(ref _isPreviewPlaying, value);
                PlayPreviewCommand.RaiseCanExecuteChanged();
                StopPreviewCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand OpenPageCommand { get; }
        public RelayCommand DownloadCommand { get; }
        public RelayCommand DownloadDirectCommand { get; }
        public RelayCommand DownloadBloodcatCommand { get; }
        public RelayCommand PlayPreviewCommand { get; }
        public RelayCommand StopPreviewCommand { get; }
        public RelayCommand TogglePreviewCommand { get; }

        public BeatmapDetailsViewModel(ISettingsService settingsService, IDataService dataService, IAudioService audioService)
        {
            _settingsService = settingsService;
            _dataService = dataService;
            _audioService = audioService;

            OpenPageCommand = new RelayCommand(OpenPage);
            DownloadCommand = new RelayCommand(Download);
            DownloadDirectCommand = new RelayCommand(DownloadDirect);
            DownloadBloodcatCommand = new RelayCommand(DownloadBloodcat);
            PlayPreviewCommand = new RelayCommand(PlayPreview, () => !IsPreviewPlaying);
            StopPreviewCommand = new RelayCommand(StopPreview, () => IsPreviewPlaying);
            TogglePreviewCommand = new RelayCommand(TogglePreview);

            MessengerInstance.Register<ShowBeatmapDetailsMessage>(this, m =>
            {
                Beatmap = m.Beatmap;
            });
        }

        private void OpenPage()
        {
            Process.Start($"https://osu.ppy.sh/b/{Beatmap.Id}");
        }

        private void Download()
        {
            var url = $"https://osu.ppy.sh/d/{Beatmap.MapSetId}";
            if (_settingsService.DownloadWithoutVideo) url += 'n';
            Process.Start(url);
        }

        private void DownloadDirect()
        {
            Process.Start($"osu://dl/{Beatmap.MapSetId}");
        }

        private void DownloadBloodcat()
        {
            Process.Start($"http://bloodcat.com/osu/s/{Beatmap.MapSetId}");
        }

        private async void PlayPreview()
        {
            IsPreviewPlaying = true;
            using (var stream = await _dataService.GetMapSetPreviewAsync(Beatmap.MapSetId))
                await _audioService.PlayAsync(stream);
            IsPreviewPlaying = false;
        }

        private async void StopPreview()
        {
            await _audioService.StopAsync();
            IsPreviewPlaying = false;
        }

        private void TogglePreview()
        {
            if (IsPreviewPlaying)
                StopPreview();
            else
                PlayPreview();
        }
    }
}