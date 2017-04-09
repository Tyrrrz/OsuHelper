using System.Diagnostics;
using System.Windows.Navigation;

namespace OsuHelper.Views
{
    public partial class SettingsDialog
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }
    }
}