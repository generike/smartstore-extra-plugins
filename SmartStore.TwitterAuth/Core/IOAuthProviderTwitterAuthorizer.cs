//Contributor:  Nicholas Mayne

using LinqToTwitter;
using SmartStore.Core.Domain.Customers;
using SmartStore.Services.Authentication.External;

namespace SmartStore.TwitterAuth.Core
{
    public interface IOAuthProviderTwitterAuthorizer : IExternalProviderAuthorizer
    {
        ITwitterAuthorizer GetAuthorizer(Customer customer);
    }
}