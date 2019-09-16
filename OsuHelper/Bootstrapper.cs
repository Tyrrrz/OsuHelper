using System.Windows;
using System.Windows.Threading;
using OsuHelper.Services;
using OsuHelper.ViewModels;
using OsuHelper.ViewModels.Framework;
using Stylet;
using StyletIoC;

namespace OsuHelper
{
    public class Bootstrapper : Bootstrapper<RootViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            base.ConfigureIoC(builder);

            // Bind settings as singleton
            builder.Bind<SettingsService>().ToSelf().InSingletonScope();

            // Bind view model factory
            builder.Bind<IViewModelFactory>().ToAbstractFactory();
        }

#if !DEBUG
        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            base.OnUnhandledException(e);

            MessageBox.Show(e.Exception.ToString(), "Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
        }
#endif
    }
}