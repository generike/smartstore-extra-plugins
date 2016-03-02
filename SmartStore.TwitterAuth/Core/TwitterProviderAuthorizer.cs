//Contributor:  Nicholas Mayne

using System;
using System.Linq;
using System.Web;
using LinqToTwitter;
using SmartStore.Core.Domain.Customers;
using SmartStore.Services;
using SmartStore.Services.Authentication.External;

namespace SmartStore.TwitterAuth.Core
{
    public class TwitterProviderAuthorizer : IOAuthProviderTwitterAuthorizer
    {
        private readonly IExternalAuthorizer _authorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly HttpContextBase _httpContext;
		private readonly ICommonServices _services;

        private IOAuthCredentials _credentials;
        private MvcAuthorizer _mvcAuthorizer;

        public TwitterProviderAuthorizer(IExternalAuthorizer authorizer,
            IOpenAuthenticationService openAuthenticationService,
            HttpContextBase httpContext,
			ICommonServices services)
        {
            this._authorizer = authorizer;
            this._openAuthenticationService = openAuthenticationService;
            this._httpContext = httpContext;
			this._services = services;
        }

        private IOAuthCredentials Credentials
        {
            get
            {
				if (_credentials == null)
				{
					var settings = _services.Settings.LoadSetting<TwitterExternalAuthSettings>(_services.StoreContext.CurrentStore.Id);

					_credentials = new SessionStateCredentials
					{
						ConsumerKey = settings.ConsumerKey,
						ConsumerSecret = settings.ConsumerSecret
					};
				}

                return _credentials;
            }
        }

        private MvcAuthorizer MvcAuthorizer
        {
            get { return _mvcAuthorizer ?? (_mvcAuthorizer = new MvcAuthorizer { Credentials = this.Credentials }); }
        }

		public AuthorizeState Authorize(string returnUrl, bool? verifyResponse = null)
        {
            //Sleep for 15 seconds as a workaround for a twitter bug. :(
            //resolve this issue because it's blocking the entire site
            //Thread.Sleep(new TimeSpan(0, 0, 0, 15));

            MvcAuthorizer.CompleteAuthorization(GenerateCallbackUri());

            if (!MvcAuthorizer.IsAuthorized)
            {
                return new AuthorizeState(returnUrl, OpenAuthenticationStatus.RequiresRedirect) { Result = MvcAuthorizer.BeginAuthorization() };
            }

            var parameters = new OAuthAuthenticationParameters(Provider.SystemName)
            {
                ExternalIdentifier = MvcAuthorizer.OAuthTwitter.OAuthToken,
                ExternalDisplayIdentifier = MvcAuthorizer.ScreenName,
                OAuthToken = MvcAuthorizer.OAuthTwitter.OAuthToken,
                OAuthAccessToken = MvcAuthorizer.OAuthTwitter.OAuthTokenSecret,
            };

            var result = _authorizer.Authorize(parameters);

            var tempReturnUrl = _httpContext.Request.QueryString["?ReturnUrl"];
            if (!string.IsNullOrEmpty(tempReturnUrl) && string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = tempReturnUrl;
            }

            return new AuthorizeState(returnUrl, result);
        }

        private Uri GenerateCallbackUri()
        {
            UriBuilder builder = new UriBuilder(_httpContext.Request.Url);
            var path = _httpContext.Request.ApplicationPath + "/Plugins/ExternalAuthTwitter/Login/";
            builder.Path = path.Replace(@"//", @"/");
            builder.Query = builder.Query.Replace(@"??", @"?");

            return builder.Uri;
        }
        public ITwitterAuthorizer GetAuthorizer(Customer customer)
        {
            var parameters = new OAuthAuthenticationParameters(Provider.SystemName);
            var identifier = _openAuthenticationService
                .GetExternalIdentifiersFor(customer)
                .Where(o => o.ProviderSystemName == parameters.ProviderSystemName)
                .ToList()
                .FirstOrDefault();

            if (identifier != null)
            {
                MvcAuthorizer.Credentials.OAuthToken = identifier.OAuthToken;
                MvcAuthorizer.Credentials.AccessToken = identifier.OAuthAccessToken;

                return MvcAuthorizer;
            }
            return null;
        }
    }
}