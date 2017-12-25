using System.Windows;
using OsuHelper.ViewModels;

namespace OsuHelper.Views
{
    public partial class BeatmapDetailsDialog
    {
        private IBeatmapDetailsViewModel ViewModel => (IBeatmapDetailsViewModel) DataContext;

        public BeatmapDetailsDialog()
        {
            InitializeComponent();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.StopPreviewCommand.Execute(null);
        }
    }
}