
using SmartStore.Core.Configuration;

namespace SmartStore.USPS
{
    public class USPSSettings : ISettings
    {
        public bool UseSandbox { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public decimal AdditionalHandlingCharge { get; set; }

        public string ZipPostalCodeFrom { get; set; }

        public string CarrierServicesOfferedDomestic { get; set; }

        public string CarrierServicesOfferedInternational { get; set; }
    }
}