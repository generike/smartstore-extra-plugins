using System.Data.Entity.Migrations;
using SmartStore.Core.Domain.Tasks;
using SmartStore.Core.Plugins;
using SmartStore.MailChimp.Data;
using SmartStore.MailChimp.Data.Migrations;
using SmartStore.Services.Localization;
using SmartStore.Services.Tasks;

namespace SmartStore.MailChimp.Services
{
	public class MailChimpInstallationService
    {
        private readonly MailChimpObjectContext _mailChimpObjectContext;
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly ILocalizationService _localizationService;

        public MailChimpInstallationService(MailChimpObjectContext mailChimpObjectContext,
            IScheduleTaskService scheduleTaskService,
            ILocalizationService localizationService)
        {
            _mailChimpObjectContext = mailChimpObjectContext;
            _scheduleTaskService = scheduleTaskService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Installs the sync task.
        /// </summary>
        private void InstallSyncTask()
        {
            //Check the database for the task
            var task = FindScheduledTask();

            if (task == null)
            {
                task = new ScheduleTask
                {
                    Name = "MailChimp sync",
					CronExpression = "0 */1 * * *", // Every hour
                    Type = "SmartStore.MailChimp.MailChimpSynchronizationTask, SmartStore.MailChimp",
                    Enabled = false,
                    StopOnError = false,
                };
                _scheduleTaskService.InsertTask(task);
            }
        }

        private ScheduleTask FindScheduledTask()
        {
            return _scheduleTaskService.GetTaskByType("SmartStore.MailChimp.MailChimpSynchronizationTask, SmartStore.MailChimp");
        }

        /// <summary>
        /// Installs this instance.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        public virtual void Install(BasePlugin plugin)
        {
            //locales
            _localizationService.ImportPluginResourcesFromXml(plugin.PluginDescriptor);

            //Install sync task
            InstallSyncTask();
        }

        /// <summary>
        /// Uninstalls this instance.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        public virtual void Uninstall(BasePlugin plugin)
        {
            //locales
            _localizationService.DeleteLocaleStringResources(plugin.PluginDescriptor.ResourceRootKey);

            //Remove scheduled task
            var task = FindScheduledTask();
            if (task != null)
                _scheduleTaskService.DeleteTask(task);

			var migrator = new DbMigrator(new Configuration());
			migrator.Update(DbMigrator.InitialDatabase);
        }
    }
}