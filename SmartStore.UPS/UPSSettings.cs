using SmartStore.Core.Configuration;
using SmartStore.UPS.Domain;

namespace SmartStore.UPS
{
    public class UPSSettings : ISettings
    {
        public string Url { get; set; }

        public string AccessKey { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public decimal AdditionalHandlingCharge { get; set; }

        public UPSCustomerClassification CustomerClassification { get; set; }

        public UPSPickupType PickupType { get; set; }

        public UPSPackagingType PackagingType { get; set; }

        public int DefaultShippedFromCountryId { get; set; }

        public string DefaultShippedFromZipPostalCode { get; set; }

        public string CarrierServicesOffered { get; set; }

        public bool InsurePackage { get; set; }
    }
}