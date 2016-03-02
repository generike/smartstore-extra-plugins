using System;
using System.Collections.Generic;
using System.Linq;
using SmartStore.Core;
using SmartStore.Core.Localization;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Plugins;
using SmartStore.Services.Discounts;
using SmartStore.Services.Localization;
using SmartStore.Services.Orders;

namespace SmartStore.DiscountRules.PurchasedProducts.Providers
{
	[SystemName("DiscountRequirement.PurchasedOneProduct")]
	[FriendlyName("Customer had previously purchased one of these products")]
	[DisplayOrder(45)]
	public partial class PurchasedOneProductRule : IDiscountRequirementRule
    {
		private readonly IOrderService _orderService;

		public PurchasedOneProductRule(IOrderService orderService)
        {
			this._orderService = orderService;
        }

		public bool CheckRequirement(CheckDiscountRequirementRequest request)
        {
			if (request == null)
				throw new ArgumentNullException("request");

			if (request.DiscountRequirement == null)
				throw new SmartException("Discount requirement is not set");

			if (String.IsNullOrWhiteSpace(request.DiscountRequirement.RestrictedProductIds))
				return true;

			if (request.Customer == null)
				return false;

			var restrictedProductIds = new List<int>();
			try
			{
				restrictedProductIds = request.DiscountRequirement.RestrictedProductIds
					.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(x => Convert.ToInt32(x))
					.ToList();
			}
			catch
			{
				//error parsing
				return false;
			}
			if (restrictedProductIds.Count == 0)
				return false;

			//purchased products
			var purchasedProducts = _orderService.GetAllOrderItems(0,
				request.Customer.Id, null, null, OrderStatus.Complete, null, null);

			bool found = false;
			foreach (var restrictedProductId in restrictedProductIds)
			{
				foreach (var purchasedProduct in purchasedProducts)
				{
					if (restrictedProductId == purchasedProduct.ProductId)
					{
						found = true;
						break;
					}
				}

				if (found)
				{
					break;
				}
			}

			if (found)
				return true;

			return false;
        }


		public string GetConfigurationUrl(int discountId, int? discountRequirementId)
		{
			string result = "Plugins/SmartStore.DiscountRules.PurchasedProducts/DiscountRules/PurchasedOneProduct?discountId={0}".FormatInvariant(discountId);
			if (discountRequirementId.HasValue)
			{
				result += string.Format("&discountRequirementId={0}", discountRequirementId.Value);
			}
			return result;
		}
	}
}