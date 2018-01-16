using System.Web.Mvc;
using SmartStore.Core.Domain.Customers;
using SmartStore.Services;
using SmartStore.Services.Authentication.External;
using SmartStore.Services.Security;
using SmartStore.TwitterAuth.Core;
using SmartStore.TwitterAuth.Models;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;
using SmartStore.Web.Framework.Settings;
using SmartStore.Core.Logging;

namespace SmartStore.TwitterAuth.Controllers
{
	public class ExternalAuthTwitterController : PluginControllerBase
    {
        private readonly IOAuthProviderTwitterAuthorizer _oAuthProviderTwitterAuthorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
		private readonly ICommonServices _services;

        public ExternalAuthTwitterController(
            IOAuthProviderTwitterAuthorizer oAuthProviderTwitterAuthorizer,
            IOpenAuthenticationService openAuthenticationService,
            ExternalAuthenticationSettings externalAuthenticationSettings,
			ICommonServices services)
        {
            this._oAuthProviderTwitterAuthorizer = oAuthProviderTwitterAuthorizer;
            this._openAuthenticationService = openAuthenticationService;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
			this._services = services;
		}

		private bool HasPermission(bool notify = true)
		{
			bool hasPermission = _services.Permissions.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods);

			if (notify && !hasPermission)
				NotifyError(_services.Localization.GetResource("Admin.AccessDenied.Description"));

			return hasPermission;
		}
        
        [AdminAuthorize, ChildActionOnly]
        public ActionResult Configure()
        {
			if (!HasPermission(false))
				return AccessDeniedPartialView();

            var model = new ConfigurationModel();
			int storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);
			var settings = _services.Settings.LoadSetting<TwitterExternalAuthSettings>(storeScope);

            model.ConsumerKey = settings.ConsumerKey;
            model.ConsumerSecret = settings.ConsumerSecret;

			var storeDependingSettingHelper = new StoreDependingSettingHelper(ViewData);
			storeDependingSettingHelper.GetOverrideKeys(settings, model, storeScope, _services.Settings);
            
            return View(model);
        }

        [HttpPost, AdminAuthorize, ChildActionOnly]
		public ActionResult Configure(ConfigurationModel model, FormCollection form)
        {
			if (!HasPermission(false))
				return Configure();

            if (!ModelState.IsValid)
                return Configure();

			var storeDependingSettingHelper = new StoreDependingSettingHelper(ViewData);
			int storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);
			var settings = _services.Settings.LoadSetting<TwitterExternalAuthSettings>(storeScope);

            settings.ConsumerKey = model.ConsumerKey;
            settings.ConsumerSecret = model.ConsumerSecret;

			storeDependingSettingHelper.UpdateSettings(settings, form, storeScope, _services.Settings);

			NotifySuccess(_services.Localization.GetResource("Admin.Common.DataSuccessfullySaved"));

			return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            return View();
        }

        public ActionResult LoginWithError()
        {
            _services.Notifier.Error(_services.Localization.GetResource("Plugins.ExternalAuth.Twitter.Error.NoCallBackUrl"));
            
            return new RedirectResult(Url.LogOn(""));
        }

        public ActionResult Login(string returnUrl)
        {
			var processor = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName(Provider.SystemName, _services.StoreContext.CurrentStore.Id);
			if (processor == null || !processor.IsMethodActive(_externalAuthenticationSettings))
			{
				throw new SmartException("Twitter module cannot be loaded");
			}

            var viewModel = new LoginModel();
            TryUpdateModel(viewModel);

            var result = _oAuthProviderTwitterAuthorizer.Authorize(returnUrl);
            switch (result.AuthenticationStatus)
            {
                case OpenAuthenticationStatus.Error:
                    {
						if (!result.Success)
						{
							foreach (var error in result.Errors)
							{
								NotifyError(error);
							}
						}

                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AssociateOnLogon:
                    {
                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AutoRegisteredEmailValidation:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.EmailValidation, returnUrl });
                    }
                case OpenAuthenticationStatus.AutoRegisteredAdminApproval:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.AdminApproval, returnUrl });
                    }
                case OpenAuthenticationStatus.AutoRegisteredStandard:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Standard, returnUrl });
                    }
                default:
                    break;
            }
            
            if (result.Result != null)
                return result.Result;
                
            return HttpContext.Request.IsAuthenticated ? RedirectToReferrer(returnUrl, "~/") : new RedirectResult(Url.LogOn(returnUrl));
        }
    }
}