using System.Web.Mvc;
using System.Web.Routing;
using SmartStore.Web.Framework.Routing;

namespace SmartStore.DiscountRules.PurchasedProducts
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
			routes.MapRoute("SmartStore.DiscountRules.PurchasedProducts",
				 "Plugins/SmartStore.DiscountRules.PurchasedProducts/{controller}/{action}",
				 new { controller = "DiscountRules", action = "Index" },
				 new[] { "SmartStore.DiscountRules.PurchasedProducts.Controllers" }
			)
			.DataTokens["area"] = "SmartStore.DiscountRules.PurchasedProducts";
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
