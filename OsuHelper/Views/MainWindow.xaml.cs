// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <MainWindow.xaml.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Navigation;
using NegativeLayer.Extensions;

namespace OsuHelper.Views
{
    public partial class MainWindow
    {
        private object _lastSelectedPopupRecommendation;

        public MainWindow()
        {
            InitializeComponent();

            // Load window settings
            if (Persistence.Default.MainWindowRect != Rect.Empty)
            {
                WindowStartupLocation = WindowStartupLocation.Manual;
                Left = Persistence.Default.MainWindowRect.X;
                Top = Persistence.Default.MainWindowRect.Y;
                Width = Persistence.Default.MainWindowRect.Width;
                Height = Persistence.Default.MainWindowRect.Height;
            }

            // Open second tab if the user completed setup
            if (Settings.Stager.Current.UserID.IsNotBlank() && Settings.Stager.Current.APIKey.IsNotBlank())
                TabControl.SelectedIndex = 1;
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Persistence.Default.MainWindowRect = new Rect(Left, Top, Width, Height);
        }

        private void MainWindow_OnLocationChanged(object sender, EventArgs e)
        {
            Persistence.Default.MainWindowRect = new Rect(Left, Top, Width, Height);
        }

        private void BeatmapListDataGrid_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (BeatmapListDataGrid.SelectedItem == null ||
                BeatmapListDataGrid.SelectedItem == _lastSelectedPopupRecommendation)
            {
                BeatmapInfoPopup.IsOpen = false;
                _lastSelectedPopupRecommendation = null;
            }
            else
            {
                BeatmapInfoPopup.IsOpen = true;
                _lastSelectedPopupRecommendation = BeatmapListDataGrid.SelectedItem;
            }
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        private void CalculatorExpectedAccuracySlider_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Slider) sender).GetBindingExpression(RangeBase.ValueProperty)?.UpdateSource();
        }
    }
}