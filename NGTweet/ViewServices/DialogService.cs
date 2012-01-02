using System;
using System.Windows;

using NGTweet.ViewModels;

namespace NGTweet.ViewServices
{
    public class DialogService : IDialogService
    {
        public void OpenWindow(Uri authorizationUri)
        {
        }

        public void ShowMessage(string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(message, "Success", MessageBoxButton.OK));
        }
    }
}