using System.IO.IsolatedStorage;

using NGTweet.ViewModels;

namespace NGTweet.ViewServices
{
    public class NGApplicationSettingsProvider : IApplicationSettingsProvider
    {
        private readonly IsolatedStorageSettings _userSettings;

        public NGApplicationSettingsProvider()
        {
            _userSettings = IsolatedStorageSettings.ApplicationSettings;
        }

        public object this[string key]
        {
            get
            {
                return _userSettings[key];
            }

            set
            {
                _userSettings[key] = value;
            }
        }

        public bool Contains(string key)
        {
            return _userSettings.Contains(key);
        }
    }
}