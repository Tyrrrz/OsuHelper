using System;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OsuHelper.Services;
using OsuHelper.ViewModels;

namespace OsuHelper
{
    public class Locator
    {
        static Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Services
            SimpleIoc.Default.Register<IDataService, OsuApiDataService>();
            SimpleIoc.Default.Register<IHttpService, HttpService>();
            SimpleIoc.Default.Register<IRecommendationService, RecommendationService>();
            SimpleIoc.Default.Register<ISettingsService, FileSettingsService>();

            // View models
            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>();
        }

        public static void Cleanup()
        {
            // ReSharper disable SuspiciousTypeConversion.Global
            (SimpleIoc.Default.GetInstance<IDataService>() as IDisposable)?.Dispose();
            (SimpleIoc.Default.GetInstance<IHttpService>() as IDisposable)?.Dispose();
            (SimpleIoc.Default.GetInstance<IRecommendationService>() as IDisposable)?.Dispose();
            (SimpleIoc.Default.GetInstance<ISettingsService>() as IDisposable)?.Dispose();

            (SimpleIoc.Default.GetInstance<IMainViewModel>() as IDisposable)?.Dispose();
            // ReSharper restore SuspiciousTypeConversion.Global
        }
    }
}