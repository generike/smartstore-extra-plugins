using System.Web.Mvc;
using SmartStore.LivePersonChat.Models;
using SmartStore.Services.Configuration;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;

namespace SmartStore.LivePersonChat.Controllers
{

    public class WidgetsLivePersonChatController : PluginControllerBase
    {
        private readonly LivePersonChatSettings _livePersonChatSettings;
        private readonly ISettingService _settingService;

        public WidgetsLivePersonChatController(LivePersonChatSettings livePersonChatSettings, ISettingService settingService)
        {
            this._livePersonChatSettings = livePersonChatSettings;
            this._settingService = settingService;
        }
        
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var model = new ConfigurationModel();
            model.ButtonCode = _livePersonChatSettings.ButtonCode;
            model.MonitoringCode = _livePersonChatSettings.MonitoringCode;

            model.ZoneId = _livePersonChatSettings.WidgetZone;
            model.AvailableZones.Add(new SelectListItem() { Text = "Before left side column", Value = "left_side_column_before" });
            model.AvailableZones.Add(new SelectListItem() { Text = "After left side column", Value = "left_side_column_after" });
            model.AvailableZones.Add(new SelectListItem() { Text = "Before right side column", Value = "right_side_column_before" });
            model.AvailableZones.Add(new SelectListItem() { Text = "After right side column", Value = "right_side_column_after" });

            return View(model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();
            
            //save settings
            _livePersonChatSettings.ButtonCode = model.ButtonCode;
            _livePersonChatSettings.MonitoringCode = model.MonitoringCode;
            _livePersonChatSettings.WidgetZone = model.ZoneId;
            _settingService.SaveSetting(_livePersonChatSettings);

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone)
        {
            var model = new PublicInfoModel();
            model.ButtonCode = _livePersonChatSettings.ButtonCode;
            model.MonitoringCode = _livePersonChatSettings.MonitoringCode;

            return View(model);
        }
    }
}