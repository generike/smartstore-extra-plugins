using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinqToTwitter;

namespace SmartStore.TwitterAuth.Core
{
	public class MvcAuthorizer : WebAuthorizer
	{
		public ActionResult BeginAuthorization()
		{
			return new MvcOAuthActionResult(this);
		}

		public new ActionResult BeginAuthorization(Uri callback)
		{
			this.Callback = callback;
			return new MvcOAuthActionResult(this);
		}
	}
}