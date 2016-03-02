using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SmartStore.Core.Domain.Discounts;
using SmartStore.DiscountRules.PurchasedProducts.Models;
using SmartStore.Services.Discounts;
using SmartStore.Services.Localization;
using SmartStore.Services.Payments;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;

namespace SmartStore.DiscountRules.PurchasedProducts.Controllers
{
	[AdminAuthorize]
    public class DiscountRulesController : PluginControllerBase
    {
        private readonly IDiscountService _discountService;

		public DiscountRulesController(IDiscountService discountService)
        {
            this._discountService = discountService;
		}

        public ActionResult PurchasedOneProduct(int discountId, int? discountRequirementId)
        {
			var discount = _discountService.GetDiscountById(discountId);
			if (discount == null)
				throw new ArgumentException("Discount could not be loaded");

			DiscountRequirement discountRequirement = null;
			if (discountRequirementId.HasValue)
			{
				discountRequirement = discount.DiscountRequirements.Where(dr => dr.Id == discountRequirementId.Value).FirstOrDefault();
				if (discountRequirement == null)
					return Content("Failed to load requirement.");
			}

			var model = new PurchasedOneProductModel();
			model.RequirementId = discountRequirementId.HasValue ? discountRequirementId.Value : 0;
			model.DiscountId = discountId;
			model.ProductIds = discountRequirement != null ? discountRequirement.RestrictedProductIds : "";

			//add a prefix
			ViewData.TemplateInfo.HtmlFieldPrefix = string.Format("DiscountRulesPurchasedOneProduct{0}", discountRequirementId.HasValue ? discountRequirementId.Value.ToString() : "0");

			return View(model);
        }

        [HttpPost]
		public ActionResult PurchasedOneProduct(int discountId, int? discountRequirementId, string productIds)
        {
			var discount = _discountService.GetDiscountById(discountId);
			if (discount == null)
				throw new ArgumentException("Discount could not be loaded");

			DiscountRequirement discountRequirement = null;
			if (discountRequirementId.HasValue)
				discountRequirement = discount.DiscountRequirements.Where(dr => dr.Id == discountRequirementId.Value).FirstOrDefault();

			if (discountRequirement != null)
			{
				//update existing rule
				discountRequirement.RestrictedProductIds = productIds;
				_discountService.UpdateDiscount(discount);
			}
			else
			{
				//save new rule
				discountRequirement = new DiscountRequirement()
				{
					DiscountRequirementRuleSystemName = "DiscountRequirement.PurchasedOneProduct",
					RestrictedProductIds = productIds,
				};
				discount.DiscountRequirements.Add(discountRequirement);
				_discountService.UpdateDiscount(discount);
			}
			return Json(new { Result = true, NewRequirementId = discountRequirement.Id }, JsonRequestBehavior.AllowGet);
        }


		public ActionResult PurchasedAllProducts(int discountId, int? discountRequirementId)
		{
			var discount = _discountService.GetDiscountById(discountId);
			if (discount == null)
				throw new ArgumentException("Discount could not be loaded");

			DiscountRequirement discountRequirement = null;
			if (discountRequirementId.HasValue)
			{
				discountRequirement = discount.DiscountRequirements.Where(dr => dr.Id == discountRequirementId.Value).FirstOrDefault();
				if (discountRequirement == null)
					return Content("Failed to load requirement.");
			}

			var model = new PurchasedAllProductsModel();
			model.RequirementId = discountRequirementId.HasValue ? discountRequirementId.Value : 0;
			model.DiscountId = discountId;
			model.ProductIds = discountRequirement != null ? discountRequirement.RestrictedProductIds : "";

			//add a prefix
			ViewData.TemplateInfo.HtmlFieldPrefix = string.Format("DiscountRulesPurchasedAllProducts{0}", discountRequirementId.HasValue ? discountRequirementId.Value.ToString() : "0");

			return View(model);
		}

		[HttpPost]
		public ActionResult PurchasedAllProducts(int discountId, int? discountRequirementId, string productIds)
		{
			var discount = _discountService.GetDiscountById(discountId);
			if (discount == null)
				throw new ArgumentException("Discount could not be loaded");

			DiscountRequirement discountRequirement = null;
			if (discountRequirementId.HasValue)
				discountRequirement = discount.DiscountRequirements.Where(dr => dr.Id == discountRequirementId.Value).FirstOrDefault();

			if (discountRequirement != null)
			{
				//update existing rule
				discountRequirement.RestrictedProductIds = productIds;
				_discountService.UpdateDiscount(discount);
			}
			else
			{
				//save new rule
				discountRequirement = new DiscountRequirement()
				{
					DiscountRequirementRuleSystemName = "DiscountRequirement.PurchasedAllProducts",
					RestrictedProductIds = productIds,
				};
				discount.DiscountRequirements.Add(discountRequirement);
				_discountService.UpdateDiscount(discount);
			}
			return Json(new { Result = true, NewRequirementId = discountRequirement.Id }, JsonRequestBehavior.AllowGet);
		}
        
    }
}