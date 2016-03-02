using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using G = Glimpse;
//using Glimpse.Ado;
//using Glimpse.EF;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using SmartStore.Core.Plugins;
using SmartStore.Core.Infrastructure;
using System.Diagnostics;

namespace SmartStore.Glimpse.Infrastructure
{
    public class GlimpseStarter : IPreApplicationStart, IStartupTask
    {

        public void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(GlimpseHttpModule));

			//G.Ado.Initialize.Start();
			//G.EF.Initialize.Start();
        }

        public void Execute()
        {
			// register Glimpse trace listener
			var listener = new G.Core.TraceListener();
			Trace.Listeners.Add(listener);
        }

        public int Order
        {
            // ensure this runs before EF initializes
            get { return -2000; }
        }
    }
}
