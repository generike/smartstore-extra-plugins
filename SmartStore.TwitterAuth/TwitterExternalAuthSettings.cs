using SmartStore.Core.Configuration;

namespace SmartStore.TwitterAuth
{
    public class TwitterExternalAuthSettings : ISettings
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
    }
}
