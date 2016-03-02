using System;
using System.Linq;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Plugins;
using SmartStore.Services.Common;
using SmartStore.Services.Discounts;
using SmartStore.Services.Localization;

namespace SmartStore.DiscountRules.PurchasedProducts
{
	public partial class Plugin : BasePlugin
    {
		private readonly ILocalizationService _localizationService;

		public Plugin(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public override void Install()
        {
            _localizationService.ImportPluginResourcesFromXml(this.PluginDescriptor);
            base.Install();
        }

        public override void Uninstall()
        {
            _localizationService.DeleteLocaleStringResources(this.PluginDescriptor.ResourceRootKey);
			_localizationService.DeleteLocaleStringResources("Plugins.DiscountRules.PurchasedOneProduct");
			_localizationService.DeleteLocaleStringResources("Plugins.DiscountRules.PurchasedAllProducts");

            base.Uninstall();
        }
    }
}