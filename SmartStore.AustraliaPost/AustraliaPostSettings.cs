
using SmartStore.Core.Configuration;

namespace SmartStore.AustraliaPost
{
    public class AustraliaPostSettings : ISettings
    {
        public string GatewayUrl { get; set; }

        public decimal AdditionalHandlingCharge { get; set; }

        public string ShippedFromZipPostalCode { get; set; }
    }
}