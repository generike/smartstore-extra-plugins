using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using SmartStore.Web.Framework.UI;
using SmartStore.Collections;

namespace SmartStore.Glimpse
{
    public class AdminMenu : AdminMenuProvider
    {
		protected override void BuildMenuCore(TreeNode<MenuItem> pluginsNode)
        {
            var menuItem = new MenuItem().ToBuilder()
                .Text("Glimpse Web Debugger")
				.ResKey("Plugins.FriendlyName.Developer.Glimpse")
				.Icon("bug")
				.Action("ConfigurePlugin", "Plugin", new { systemName = "SmartStore.Glimpse", area = "Admin" })
                .ToItem();

            pluginsNode.Prepend(menuItem);
        }

    }
}
