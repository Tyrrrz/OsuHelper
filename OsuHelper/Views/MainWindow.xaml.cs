using System.Reflection;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Tyrrrz.Extensions;

namespace OsuHelper.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // Version in title
            Title = Title.Format(Assembly.GetEntryAssembly().GetName().Version);
        }

        private void ShowSettingsDialogButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogHost.Show(new SettingsDialog()).Forget();
        }
    }
}