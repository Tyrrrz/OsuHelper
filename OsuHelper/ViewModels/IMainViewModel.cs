using System.Collections.Generic;
using GalaSoft.MvvmLight.CommandWpf;
using OsuHelper.Models;

namespace OsuHelper.ViewModels
{
    public interface IMainViewModel
    {
        bool IsBusy { get; }

        IReadOnlyList<BeatmapRecommendation> Recommendations { get; }

        bool HasData { get; }

        RelayCommand GetRecommendationsCommand { get; }
    }
}