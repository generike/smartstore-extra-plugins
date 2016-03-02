﻿using System.Collections.Generic;
using System.Web.Mvc;
using SmartStore.Web.Framework;
﻿using SmartStore.Web.Framework.Modelling;

namespace SmartStore.LivePersonChat.Models
{
    public class ConfigurationModel : ModelBase
    {
         public ConfigurationModel()
        {
            AvailableZones = new List<SelectListItem>();
        }

        [SmartResourceDisplayName("Admin.ContentManagement.Widgets.ChooseZone")]
        public string ZoneId { get; set; }
        public IList<SelectListItem> AvailableZones { get; set; }

        [SmartResourceDisplayName("Plugins.Widgets.LivePersonChat.ButtonCode")]
        [AllowHtml]
        public string ButtonCode { get; set; }

        [SmartResourceDisplayName("Plugins.Widgets.LivePersonChat.MonitoringCode")]
        [AllowHtml]
        public string MonitoringCode { get; set; }
    }
}