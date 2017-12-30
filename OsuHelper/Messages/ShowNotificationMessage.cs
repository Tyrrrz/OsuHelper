namespace OsuHelper.Messages
{
    public class ShowNotificationMessage
    {
        public string Title { get; }

        public string Content { get; }

        public ShowNotificationMessage(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}