using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using SmartStore.Web.Framework.UI;
using SmartStore.Collections;

namespace SmartStore.Mailchimp
{
    public class AdminMenu : AdminMenuProvider
    {

		protected override void BuildMenuCore(TreeNode<MenuItem> pluginsNode)
		{
			var menuItem = new MenuItem().ToBuilder()
				.Text("MailChimp E-Mail Synchronization")
				.ResKey("Plugins.FriendlyName.Misc.MailChimp")
				//.Icon("bug")
				.Action("ConfigurePlugin", "Plugin", new { systemName = "SmartStore.Mailchimp", area = "Admin" })
				.ToItem();

			pluginsNode.Prepend(menuItem);
		}

    }
}
