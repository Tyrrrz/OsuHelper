// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Locator.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

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

            SimpleIoc.Default.Register<APIService>();
            SimpleIoc.Default.Register<WindowService>();

            SimpleIoc.Default.Register<SetupViewModel>();
            SimpleIoc.Default.Register<RecommenderViewModel>();
        }

        public SetupViewModel SetupViewModel => ServiceLocator.Current.GetInstance<SetupViewModel>();
        public RecommenderViewModel RecommenderViewModel => ServiceLocator.Current.GetInstance<RecommenderViewModel>();
    }
}