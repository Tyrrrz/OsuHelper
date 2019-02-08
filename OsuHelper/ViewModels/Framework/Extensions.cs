using OsuHelper.Models;
using OsuHelper.ViewModels.Dialogs;

namespace OsuHelper.ViewModels.Framework
{
    public static class Extensions
    {
        public static BeatmapDetailsViewModel CreateBeatmapDetailsViewModel(this IViewModelFactory factory,
            Beatmap beatmap)
        {
            var viewModel = factory.CreateBeatmapDetailsViewModel();
            viewModel.Beatmap = beatmap;

            return viewModel;
        }
    }
}