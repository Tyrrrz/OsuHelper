using System.Collections.Generic;
using GalaSoft.MvvmLight.CommandWpf;
using OsuHelper.Models;

namespace OsuHelper.ViewModels
{
    public interface IMainViewModel
    {
        bool IsBusy { get; }

        IEnumerable<BeatmapRecommendation> Recommendations { get; }

        RelayCommand GetRecommendationsCommand { get; }
    }
}