using SmartStore.Core.Configuration;

namespace SmartStore.AuthorizeNet
{
    public class AuthorizeNetSettings : ISettings
    {
        public bool UseSandbox { get; set; }
        public TransactMode TransactMode { get; set; }
        public string TransactionKey { get; set; }
        public string LoginId { get; set; }
        public decimal AdditionalFee { get; set; }
    }
}
