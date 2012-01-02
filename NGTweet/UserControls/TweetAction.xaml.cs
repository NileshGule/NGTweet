namespace NGTweet.UserControls
{
    public partial class TweetAction
    {
        public TweetAction()
        {
            InitializeComponent();

            //Messenger.Default.Register<string>(this, "SendTweetSuccess", DisplayNotification);
        }

        //private void DisplayNotification(string successMessage)
        //{
        //    NotificationWindow notificationWindow = new NotificationWindow();

        //    notificationWindow.Height = 100;
        //    notificationWindow.Width = 200;

        //    Border border = new Border()
        //    {
        //        Background = new SolidColorBrush(Colors.Gray),
        //        Height = notificationWindow.Height,
        //        Width = notificationWindow.Width,
        //        Child = new TextBlock()
        //        {
        //            Text = "This is a Custom Notification from Silverlight 4",
        //            Foreground = new SolidColorBrush(Colors.White)
        //        }
        //    };

        //    notificationWindow.Content = border;

        //    notificationWindow.Show((int)TimeSpan.FromSeconds(2).TotalMilliseconds);
        //}
    }
}