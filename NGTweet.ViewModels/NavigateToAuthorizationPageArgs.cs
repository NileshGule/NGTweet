using System;

namespace NGTweet.ViewModels
{
    public class NavigateToAuthorizationPageArgs : EventArgs
    {
        public Uri AuthorizationUri { get; set; }
    }
}