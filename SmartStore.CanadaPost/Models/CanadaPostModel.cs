using SmartStore.Web.Framework;

namespace SmartStore.CanadaPost.Models
{
    public class CanadaPostModel
    {
        [SmartResourceDisplayName("Plugins.Shipping.CanadaPost.Fields.Url")]
        public string Url { get; set; }

        [SmartResourceDisplayName("Plugins.Shipping.CanadaPost.Fields.Port")]
        public int Port { get; set; }

        [SmartResourceDisplayName("Plugins.Shipping.CanadaPost.Fields.CustomerId")]
        public string CustomerId { get; set; }
    }
}