using System.Web.Mvc;
using System.Web.Routing;
using SmartStore.TwitterAuth.Core;
using SmartStore.Web.Framework.Routing;

namespace SmartStore.TwitterAuth
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
			routes.MapRoute("SmartStore.TwitterAuth",
				 "Plugins/SmartStore.TwitterAuth/{action}",
                 new { controller = "ExternalAuthTwitter", action = "Configure" },
                 new[] { "SmartStore.TwitterAuth.Controllers" }
            )
			.DataTokens["area"] = Provider.SystemName;
        }

        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
