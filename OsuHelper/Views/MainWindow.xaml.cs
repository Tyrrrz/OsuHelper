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
        private CollectionViewSource RecommendationsView => (CollectionViewSource) Resources["RecommendationsView"];

        public MainWindow()
        {
            InitializeComponent();
            Title += $" v{Assembly.GetExecutingAssembly().GetName().Version}";

            Snackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(5));

            // Notification messages
            Messenger.Default.Register<ShowNotificationMessage>(this,
                m => Snackbar.MessageQueue.Enqueue(m.Message, m.CallbackCaption, m.Callback));

            // Dialog messages
            Messenger.Default.Register<ShowBeatmapDetailsMessage>(this, m =>
            {
                DialogHost.Show(new BeatmapDetailsDialog()).Forget();
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

            e.Accepted = true;

            if (rec.Mods == Mods.None)
                e.Accepted &= NomodFilterCheckBox.IsChecked == true;

            if (rec.Mods.HasFlag(Mods.Hidden))
                e.Accepted &= HiddenFilterCheckBox.IsChecked == true;

            if (rec.Mods.HasFlag(Mods.HardRock))
                e.Accepted &= HardRockFilterCheckBox.IsChecked == true;

            if (rec.Mods.HasFlag(Mods.DoubleTime))
                e.Accepted &= DoubleTimeFilterCheckBox.IsChecked == true;

            var modsOther = rec.Mods & ~Mods.Hidden & ~Mods.HardRock & ~Mods.DoubleTime;
            if (modsOther != Mods.None)
                e.Accepted &= OtherFilterCheckBox.IsChecked == true;
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

        private void OtherFilterCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            UpdateRecommendationsView();
        }

        private void OtherFilterCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            UpdateRecommendationsView();
        }
    }
}