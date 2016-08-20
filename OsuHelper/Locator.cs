// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Locator.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OsuHelper.ViewModels;

namespace OsuHelper
{
    public class Locator
    {
        static Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<SetupViewModel>();
        }

        public Settings Settings => Settings.Default;
        public SetupViewModel SetupViewModel => ServiceLocator.Current.GetInstance<SetupViewModel>();
    }
}