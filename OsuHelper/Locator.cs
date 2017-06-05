using System;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OsuHelper.Services;
using OsuHelper.ViewModels;

namespace OsuHelper
{
    public class Locator
    {
        public static void Init()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Services
            SimpleIoc.Default.Register<IAudioService, AudioService>();
            SimpleIoc.Default.Register<IBeatmapProcessorService, OppaiBeatmapProcessorService>();
            SimpleIoc.Default.Register<ICacheService, FileCacheService>();
            SimpleIoc.Default.Register<IDataService, OsuWebDataService>();
            SimpleIoc.Default.Register<IHttpService, HttpService>();
            SimpleIoc.Default.Register<IRecommendationService, RecommendationService>();
            SimpleIoc.Default.Register<ISettingsService, FileSettingsService>();

            // View models
            SimpleIoc.Default.Register<IBeatmapDetailsViewModel, BeatmapDetailsViewModel>();
            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>();
            SimpleIoc.Default.Register<ISettingsViewModel, SettingsViewModel>();

            // Load settings
            ServiceLocator.Current.GetInstance<ISettingsService>().Load();
        }

        public static void Cleanup()
        {
            // Save settings
            ServiceLocator.Current.GetInstance<ISettingsService>().Save();

            // ReSharper disable SuspiciousTypeConversion.Global
            (ServiceLocator.Current.GetInstance<IAudioService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IBeatmapProcessorService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<ICacheService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IDataService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IHttpService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IRecommendationService>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<ISettingsService>() as IDisposable)?.Dispose();

            (ServiceLocator.Current.GetInstance<IBeatmapDetailsViewModel>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<IMainViewModel>() as IDisposable)?.Dispose();
            (ServiceLocator.Current.GetInstance<ISettingsViewModel>() as IDisposable)?.Dispose();
            // ReSharper restore SuspiciousTypeConversion.Global
        }

        public IBeatmapDetailsViewModel BeatmapDetailsViewModel => ServiceLocator.Current.GetInstance<IBeatmapDetailsViewModel>();
        public IMainViewModel MainViewModel => ServiceLocator.Current.GetInstance<IMainViewModel>();
        public ISettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<ISettingsViewModel>();
    }
}