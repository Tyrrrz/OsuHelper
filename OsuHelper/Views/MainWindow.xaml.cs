// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <MainWindow.xaml.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System;
using System.Windows;

namespace OsuHelper.Views
{
    public partial class MainWindow
    {
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
        }

        private void BeatmapListDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            BeatmapInfoPopup.IsOpen = false; // setting it to false first resets its position
            if (BeatmapListDataGrid.SelectedItem != null)
                BeatmapInfoPopup.IsOpen = true;
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Persistence.Default.MainWindowRect = new Rect(Left, Top, Width, Height);
        }

        private void MainWindow_OnLocationChanged(object sender, EventArgs e)
        {
            Persistence.Default.MainWindowRect = new Rect(Left, Top, Width, Height);
        }
    }
}