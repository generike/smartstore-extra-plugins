using Autofac;
using SmartStore.Core.Infrastructure;
using SmartStore.Core.Infrastructure.DependencyManagement;
using SmartStore.Services.Authentication.External;
using SmartStore.TwitterAuth.Core;

namespace SmartStore.TwitterAuth
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, bool isActiveModule)
        {
            builder.RegisterType<TwitterProviderAuthorizer>().Named<IExternalProviderAuthorizer>("twitter").InstancePerRequest();
        }
    }
}
