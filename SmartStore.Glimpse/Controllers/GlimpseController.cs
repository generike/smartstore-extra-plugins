using System;
using System.Linq;
using System.Web.Mvc;
using SmartStore.Core;
using SmartStore.Utilities;
using SmartStore.Services.Configuration;
using SmartStore.Services.Localization;
using SmartStore.Services.Stores;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Settings;
using SmartStore.Web.Framework.UI;
using Glimpse.Core.Framework;
using System.Web;
using SmartStore.Web.Framework.Security;

namespace SmartStore.Glimpse.Controllers
{

	public class GlimpseController : PluginControllerBase
    {
		private readonly IWorkContext _workContext;
		private readonly IStoreContext _storeContext;
		private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;

        public GlimpseController(
            IWorkContext workContext,
			IStoreContext storeContext, 
            IStoreService storeService,
            ISettingService settingService)
        {
			_workContext = workContext;
			_storeContext = storeContext;
			_storeService = storeService;
            _settingService = settingService;
        }

        public ActionResult Index([Bind(Prefix = "n")] string resourceName)
        {
            var httpContext = this.HttpContext;

            // HACK: This is a hack for the POC. We will update Glimpse to allow for calling GlimpseRuntime.Instance instead.
            var runtime = httpContext.Application["__GlimpseRuntime"] as IGlimpseRuntime;
            if (runtime == null)
            {
                throw new HttpException(404, "Glimpse runtime is missing.");
            }

            if (resourceName.IsEmpty())
            {
                runtime.ExecuteDefaultResource();
            }
            else
            {
                runtime.ExecuteResource(resourceName, new ResourceParameters(httpContext.Request.QueryString.ToDictionary()));
            }

			return new EmptyResult();
        }

        [ChildActionOnly]
		[AdminAuthorize]
        public ActionResult Configure()
        {
			// load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var model = _settingService.LoadSetting<GlimpseSettings>(storeScope);

			var storeDependingSettingHelper = new StoreDependingSettingHelper(ViewData);
            storeDependingSettingHelper.GetOverrideKeys(model, model, storeScope, _settingService);

            return View(model);
        }

        [HttpPost]
		[AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(GlimpseSettings model, FormCollection form)
        {
            if (!ModelState.IsValid)
                return Configure();

			ModelState.Clear();

            // load settings for a chosen store scope
            var storeDependingSettingHelper = new StoreDependingSettingHelper(ViewData);
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);

            storeDependingSettingHelper.UpdateSettings(model /*settings*/, form, storeScope, _settingService);
            _settingService.ClearCache();

            return Configure();
        }

    }
}