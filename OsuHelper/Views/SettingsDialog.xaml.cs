using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Navigation;
using MaterialDesignThemes.Wpf;

namespace OsuHelper.Views
{
    public partial class SettingsDialog
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void SettingsDialog_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
        }
    }
}