using LinqToTwitter;
using SmartStore.Core.Infrastructure;
using SmartStore.Core.Logging;
using SmartStore.Services;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartStore.TwitterAuth.Core
{
    public class MvcOAuthActionResult : ActionResult
	{
		private readonly WebAuthorizer _webAuth;
        private readonly ICommonServices _services;

        public MvcOAuthActionResult(WebAuthorizer webAuth)
		{
			_webAuth = webAuth;
            _services = EngineContext.Current.Resolve<ICommonServices>();

            var factory = EngineContext.Current.Resolve<ILoggerFactory>(); 
            Logger = factory.GetLogger(typeof(Log4netLogger));
        }

        public ILogger Logger { get; set; }

        public override void ExecuteResult(ControllerContext context)
		{
            _webAuth.PerformRedirect = authUrl =>
			{
				HttpContext.Current.Response.Redirect(authUrl);
			};

			Uri callback = _webAuth.Callback == null ? HttpContext.Current.Request.Url : _webAuth.Callback;

            try
            {
                _webAuth.BeginAuthorization(callback);
            } 
            catch(Exception ex)
            {
                Logger.Error(ex, _services.Localization.GetResource("Plugins.ExternalAuth.Twitter.Error.NoCallBackUrl"));
                
                var urlHelper = new UrlHelper(context.RequestContext);
                var url = urlHelper.Action("LoginWithError", "ExternalAuthTwitter", new { area = "SmartStore.TwitterAuth" });

                HttpContext.Current.Response.Redirect(url);
            }
        }
	}
}