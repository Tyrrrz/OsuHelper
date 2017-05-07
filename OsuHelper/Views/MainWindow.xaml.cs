using System.Reflection;
using System.Windows;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using OsuHelper.Models;
using Tyrrrz.Extensions;

namespace OsuHelper.Views
{
    public partial class MainWindow
    {
        private CollectionViewSource RecommendationsViewSource => (CollectionViewSource) Resources[
            "RecommendationsView"];

        public MainWindow()
        {
            InitializeComponent();

            // Version in title
            Title = Title.Format(Assembly.GetEntryAssembly().GetName().Version);
        }

        private void RecommendationsView_OnFilter(object sender, FilterEventArgs e)
        {
            if (!IsInitialized) return;

            var rec = (BeatmapRecommendation) e.Item;

            if (NomodFilterCheckBox.IsChecked == false && rec.Mods == Mods.None)
                e.Accepted = false;
            else if (HiddenFilterCheckBox.IsChecked == false && rec.Mods.HasFlag(Mods.Hidden))
                e.Accepted = false;
            else if (HardrockFilterCheckBox.IsChecked == false && rec.Mods.HasFlag(Mods.HardRock))
                e.Accepted = false;
            else if (DoubletimeFilterCheckBox.IsChecked == false && rec.Mods.HasFlag(Mods.DoubleTime))
                e.Accepted = false;
            else
                e.Accepted = true;
        }

        private void ShowSettingsDialogButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogHost.Show(new SettingsDialog()).Forget();
        }

        private void NomodFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            RecommendationsViewSource.View.Refresh();
        }

        private void NomodFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            RecommendationsViewSource.View.Refresh();
        }

        private void HiddenFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            RecommendationsViewSource.View.Refresh();
        }

        private void HiddenFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            RecommendationsViewSource.View.Refresh();
        }

        private void HardrockFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            RecommendationsViewSource.View.Refresh();
        }

        private void HardrockFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            RecommendationsViewSource.View.Refresh();
        }

        private void DoubletimeFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            RecommendationsViewSource.View.Refresh();
        }

        private void DoubletimeFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            RecommendationsViewSource.View.Refresh();
        }
    }
}