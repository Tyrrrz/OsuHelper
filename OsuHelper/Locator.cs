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
            SimpleIoc.Default.Register<OppaiService>();
            SimpleIoc.Default.Register<WindowService>();

            SimpleIoc.Default.Register<SetupViewModel>();
            SimpleIoc.Default.Register<RecommenderViewModel>();
            SimpleIoc.Default.Register<CalculatorViewModel>();
        }

        public static T Resolve<T>() => ServiceLocator.Current.GetInstance<T>();

        public SetupViewModel SetupViewModel => Resolve<SetupViewModel>();
        public RecommenderViewModel RecommenderViewModel => Resolve<RecommenderViewModel>();
        public CalculatorViewModel CalculatorViewModel => Resolve<CalculatorViewModel>();
    }
}