namespace NGTweet.ViewModels
{
    public interface IApplicationSettingsProvider
    {
        object this[string key]
        {
            get;
            set;
        }

        bool Contains(string key);
    }
}