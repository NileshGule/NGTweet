using System;
using System.Windows;
using System.Windows.Browser;

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

using NGTweet.ViewModel;
using NGTweet.ViewModels;

namespace NGTweet
{
    public partial class MainPage
    {
        public MainPage()
        {
            ViewModelLocator locator = new ViewModelLocator();

            DataContext = locator.MainViewModel;

            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private static void OnAuthorizationCompleted(DialogMessage dialogMessage)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            MessageBox.Show(dialogMessage.Content, dialogMessage.Caption, dialogMessage.Button));
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel viewModel = DataContext as MainViewModel;

            viewModel.Login();

            Messenger.Default.Register<Uri>(this, NavigateToAuthorizationPage);

            Messenger.Default.Register<DialogMessage>(this, "AuthorizationCompleted", OnAuthorizationCompleted);
        }

        private void NavigateToAuthorizationPage(Uri navigationUri)
        {
            string javaScript = string.Format("window.open('{0}', '_blank', '', '')", navigationUri);

            Dispatcher.BeginInvoke(() => HtmlPage.Window.Eval(javaScript));
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Exception errorException = e.ErrorException;
        }
    }
}