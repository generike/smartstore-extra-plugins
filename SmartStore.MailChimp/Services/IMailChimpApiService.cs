using System.Collections.Generic;
using System.Collections.Specialized;
using MailChimp.Lists;
using SmartStore.MailChimp.Data;

namespace SmartStore.MailChimp.Services
{
    public interface IMailChimpApiService
    {
        /// <summary>
        /// Retrieves the lists.
        /// </summary>
        /// <returns></returns>
        NameValueCollection RetrieveLists();

        /// <summary>
        /// Synchronize the subscription.
        /// </summary>
        /// <returns>Result</returns>
        SyncResult Synchronize();

        /// <summary>
        /// Batches the unsubscribe.
        /// </summary>
        /// <param name="recordList">The records</param>
        BatchUnsubscribeResult BatchUnsubscribe(IEnumerable<MailChimpEventQueueRecord> recordList);

        /// <summary>
        /// Batches the subscribe.
        /// </summary>
        /// <param name="recordList">The records</param>
        BatchSubscribeResult BatchSubscribe(IEnumerable<MailChimpEventQueueRecord> recordList);
    }
}