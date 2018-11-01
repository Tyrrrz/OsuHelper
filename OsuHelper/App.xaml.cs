using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;

namespace OsuHelper
{
    public partial class App
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        private void App_OnStartup(object sender, StartupEventArgs args)
        {
            Container.Init();
        }

        private void App_OnExit(object sender, ExitEventArgs args)
        {
            Container.Cleanup();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            MessageBox.Show(args.Exception.ToString(), "Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}