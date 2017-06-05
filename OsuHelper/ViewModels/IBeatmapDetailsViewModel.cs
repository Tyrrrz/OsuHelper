using GalaSoft.MvvmLight.CommandWpf;
using OsuHelper.Models;

namespace OsuHelper.ViewModels
{
    public interface IBeatmapDetailsViewModel
    {
        Beatmap Beatmap { get; set; }
        bool IsPreviewPlaying { get; }

        RelayCommand OpenPageCommand { get; }
        RelayCommand DownloadCommand { get; }
        RelayCommand DownloadDirectCommand { get; }
        RelayCommand DownloadBloodcatCommand { get; }
        RelayCommand PlayPreviewCommand { get; }
        RelayCommand StopPreviewCommand { get; }
    }
}