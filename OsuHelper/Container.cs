using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OsuHelper.Services;
using OsuHelper.ViewModels;

namespace OsuHelper
{
    public class Container
    {
        public static void Init()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Services
            SimpleIoc.Default.Register<IAudioService, AudioService>();
            SimpleIoc.Default.Register<IBeatmapProcessorService, BeatmapProcessorService>();
            SimpleIoc.Default.Register<ICacheService, CacheService>();
            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<IRecommendationService, RecommendationService>();
            SimpleIoc.Default.Register<ISettingsService, SettingsService>();
            SimpleIoc.Default.Register<IUpdateService, UpdateService>();

            // View models
            SimpleIoc.Default.Register<IBeatmapDetailsViewModel, BeatmapDetailsViewModel>(true);
            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>(true);
            SimpleIoc.Default.Register<ISettingsViewModel, SettingsViewModel>(true);
        }

        public static void Cleanup()
        {
        }

        public IBeatmapDetailsViewModel BeatmapDetailsViewModel => ServiceLocator.Current.GetInstance<IBeatmapDetailsViewModel>();
        public IMainViewModel MainViewModel => ServiceLocator.Current.GetInstance<IMainViewModel>();
        public ISettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<ISettingsViewModel>();
    }
}