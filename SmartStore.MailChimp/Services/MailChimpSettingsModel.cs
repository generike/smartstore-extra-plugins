
using System.Collections.Generic;

namespace SmartStore.MailChimp.Services
{
    public class SyncResult
    {
        public SyncResult()
        {
            SubscribeErrors = new List<string>();
            UnsubscribeErrors = new List<string>();
        }
        public string SubscribeResult { get; set; }
        public IList<string> SubscribeErrors { get; set; }
        public string UnsubscribeResult { get; set; }
        public IList<string> UnsubscribeErrors { get; set; }
    }
}