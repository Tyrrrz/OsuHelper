using System.Reflection;
using Tyrrrz.Extensions;

namespace OsuHelper.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // Version in title
            Title = Title.Format(Assembly.GetEntryAssembly().GetName().Version);
        }
    }
}