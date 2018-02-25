using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace OsuHelper
{
    public partial class App
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Container.Init();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Container.Cleanup();
        }
    }
}