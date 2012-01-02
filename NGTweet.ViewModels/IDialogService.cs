using System;

namespace NGTweet.ViewModels
{
    public interface IDialogService
    {
        void OpenWindow(Uri authorizationUri);

        void ShowMessage(string message);
    }
}