using System.Reflection;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using OsuHelper.Messages;
using OsuHelper.Models;
using Tyrrrz.Extensions;

namespace OsuHelper.Views
{
    public partial class MainWindow
    {
        private CollectionViewSource RecommendationsView => (CollectionViewSource) Resources["RecommendationsView"];

        public MainWindow()
        {
            InitializeComponent();

            // Version in title
            Title = string.Format(Title, Assembly.GetEntryAssembly().GetName().Version.ToString(3));

            // Dialogs
            Messenger.Default.Register<ShowBeatmapDetailsMessage>(this, m =>
            {
                DialogHost.Show(new BeatmapDetailsDialog()).Forget();
            });
            Messenger.Default.Register<ShowNotificationMessage>(this, m =>
            {
                DialogHost.Show(new NotificationDialog()).Forget();
            });
            Messenger.Default.Register<ShowSettingsMessage>(this, m =>
            {
                DialogHost.Show(new SettingsDialog()).Forget();
            });
        }

        private void UpdateRecommendationsView()
        {
            RecommendationsView.View?.Refresh();
        }

        private void RecommendationsView_OnFilter(object sender, FilterEventArgs e)
        {
            if (!IsInitialized) return;

            var rec = (Recommendation) e.Item;

            if (NomodFilterCheckBox.IsChecked == false && rec.Mods == Mods.None)
                e.Accepted = false;
            else if (HiddenFilterCheckBox.IsChecked == false && rec.Mods.HasFlag(Mods.Hidden))
                e.Accepted = false;
            else if (HardRockFilterCheckBox.IsChecked == false && rec.Mods.HasFlag(Mods.HardRock))
                e.Accepted = false;
            else if (DoubleTimeFilterCheckBox.IsChecked == false && rec.Mods.HasFlag(Mods.DoubleTime))
                e.Accepted = false;
            else
                e.Accepted = true;
        }

        private void NomodFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            UpdateRecommendationsView();
        }

        private void NomodFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            UpdateRecommendationsView();
        }

        private void HiddenFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            UpdateRecommendationsView();
        }

        private void HiddenFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            UpdateRecommendationsView();
        }

        private void HardRockFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            UpdateRecommendationsView();
        }

        private void HardRockFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            UpdateRecommendationsView();
        }

        private void DoubleTimeFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            UpdateRecommendationsView();
        }

        private void DoubleTimeFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            UpdateRecommendationsView();
        }
    }
}