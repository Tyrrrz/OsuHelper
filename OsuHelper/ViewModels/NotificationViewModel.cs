using GalaSoft.MvvmLight;
using OsuHelper.Messages;

namespace OsuHelper.ViewModels
{
    public class NotificationViewModel : ViewModelBase, INotificationViewModel
    {
        private string _title;
        private string _content;

        public string Title
        {
            get => _title;
            private set => Set(ref _title, value);
        }

        public string Content
        {
            get => _content;
            private set => Set(ref _content, value);
        }

        public NotificationViewModel()
        {
            // Messages
            MessengerInstance.Register<ShowNotificationMessage>(this, m =>
            {
                Title = m.Title;
                Content = m.Content;
            });
        }
    }
}