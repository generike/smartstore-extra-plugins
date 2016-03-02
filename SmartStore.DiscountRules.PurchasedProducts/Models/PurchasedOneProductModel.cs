using System.Collections.Generic;
using System.Web.Mvc;
using SmartStore.Web.Framework;

namespace SmartStore.DiscountRules.PurchasedProducts.Models
{
    public class PurchasedOneProductModel
    {
		[SmartResourceDisplayName("Plugins.DiscountRules.PurchasedOneProduct.Fields.Products")]
		public string ProductIds { get; set; }
		public int DiscountId { get; set; }
		public int RequirementId { get; set; }
    }
}