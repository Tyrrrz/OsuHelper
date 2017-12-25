using System;
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

        private void RecommendationsView_OnFilter(object sender, FilterEventArgs e)
        {
            if (!IsInitialized) return;

            var rec = (Recommendation) e.Item;

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

        private void NomodFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            ((CollectionViewSource) Resources["RecommendationsView"]).View?.Refresh();
        }

        private void NomodFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ((CollectionViewSource) Resources["RecommendationsView"]).View?.Refresh();
        }

        private void HiddenFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            ((CollectionViewSource) Resources["RecommendationsView"]).View?.Refresh();
        }

        private void HiddenFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ((CollectionViewSource) Resources["RecommendationsView"]).View?.Refresh();
        }

        private void HardrockFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            ((CollectionViewSource) Resources["RecommendationsView"]).View?.Refresh();
        }

        private void HardrockFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ((CollectionViewSource) Resources["RecommendationsView"]).View?.Refresh();
        }

        private void DoubletimeFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            ((CollectionViewSource) Resources["RecommendationsView"]).View?.Refresh();
        }

        private void DoubletimeFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ((CollectionViewSource) Resources["RecommendationsView"]).View?.Refresh();
        }
    }
}