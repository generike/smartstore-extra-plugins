using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToTwitter;

namespace SmartStore.TwitterAuth.Core
{
	public class MvcOAuthActionResult : ActionResult
	{
		private readonly WebAuthorizer webAuth;

		public MvcOAuthActionResult(WebAuthorizer webAuth)
		{
			this.webAuth = webAuth;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			webAuth.PerformRedirect = authUrl =>
			{
				HttpContext.Current.Response.Redirect(authUrl);
			};

			Uri callback =
				webAuth.Callback == null ?
					HttpContext.Current.Request.Url :
					webAuth.Callback;

			webAuth.BeginAuthorization(callback);
		}
	}
}