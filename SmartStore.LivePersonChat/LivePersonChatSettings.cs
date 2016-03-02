using SmartStore.Core.Configuration;

namespace SmartStore.LivePersonChat	
{
    public class LivePersonChatSettings : ISettings
    {
        public string ButtonCode { get; set; }
        public string MonitoringCode { get; set; }
        public string WidgetZone { get; set; }
    }
}