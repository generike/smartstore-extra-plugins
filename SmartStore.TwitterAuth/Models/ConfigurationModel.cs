using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;

namespace SmartStore.TwitterAuth.Models
{
    public class ConfigurationModel : ModelBase
    {
        [SmartResourceDisplayName("Plugins.ExternalAuth.Twitter.ConsumerKey")]
        public string ConsumerKey { get; set; }

        [SmartResourceDisplayName("Plugins.ExternalAuth.Twitter.ConsumerSecret")]
        public string ConsumerSecret { get; set; }
    }
}