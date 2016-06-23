using System.Diagnostics;
//using Glimpse.Ado;
//using Glimpse.EF;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using SmartStore.Core.Infrastructure;
using SmartStore.Core.Plugins;
using G = Glimpse;

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
