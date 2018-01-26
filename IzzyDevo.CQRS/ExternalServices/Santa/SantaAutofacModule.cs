using Autofac;
using IzzyDevo.CQRS.ExternalServices.Santa.Contracts;

namespace IzzyDevo.CQRS.ExternalServices.Santa
{
    public class SantaAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<SantaAPI>()
                .As<ISantaAPI>()
                .InstancePerLifetimeScope();
        }
    }
}
