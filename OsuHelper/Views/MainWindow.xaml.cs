using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using OsuHelper.Models;
using OsuHelper.ViewModels;
using Tyrrrz.Extensions;

namespace OsuHelper.Views
{
    public partial class MainWindow
    {
        private readonly ICollectionView _recommendationsView;

        private IMainViewModel ViewModel => (IMainViewModel) DataContext;

        public MainWindow()
        {
            InitializeComponent();

            // Collection view
            _recommendationsView = CollectionViewSource.GetDefaultView(ViewModel.Recommendations);
            _recommendationsView.Filter = o =>
            {
                var rec = (BeatmapRecommendation) o;
                if (NomodFilterCheckBox.IsChecked == false && rec.Mods == Mods.None)
                    return false;
                if (HiddenFilterCheckBox.IsChecked == false && rec.Mods.HasFlag(Mods.Hidden))
                    return false;
                if (HardrockFilterCheckBox.IsChecked == false && rec.Mods.HasFlag(Mods.HardRock))
                    return false;
                if (DoubletimeFilterCheckBox.IsChecked == false && rec.Mods.HasFlag(Mods.DoubleTime))
                    return false;

                return true;
            };

            // Version in title
            Title = Title.Format(Assembly.GetEntryAssembly().GetName().Version);
        }

        private void ShowSettingsDialogButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogHost.Show(new SettingsDialog()).Forget();
        }

        private void NomodFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            _recommendationsView?.Refresh();
        }

        private void NomodFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _recommendationsView?.Refresh();
        }

        private void HiddenFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            _recommendationsView?.Refresh();
        }

        private void HiddenFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _recommendationsView?.Refresh();
        }

        private void HardrockFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            _recommendationsView?.Refresh();
        }

        private void HardrockFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _recommendationsView?.Refresh();
        }

        private void DoubletimeFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            _recommendationsView?.Refresh();
        }

        private void DoubletimeFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _recommendationsView?.Refresh();
        }
    }
}