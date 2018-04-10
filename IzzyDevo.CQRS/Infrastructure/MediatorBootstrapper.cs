using System.Collections.Generic;
using System.Reflection;
using Autofac;
using IzzyDevo.CQRS.ExternalServices.Santa;
using IzzyDevo.CQRS.Infrastructure.Database;
using IzzyDevo.CQRS.Infrastructure.Log;
using IzzyDevo.CQRS.Infrastructure.Pipeline.Logging;
using IzzyDevo.CQRS.Infrastructure.Pipeline.Transactional;
using MediatR;
using MediatR.Pipeline;
using Raven.Client;

namespace IzzyDevo.CQRS.Infrastructure
{
    public class MediatorBootstrapper
    {
        public IMediator BuildMediator()
        {
            var container = ConfigureContainer();
            var mediator = container.Resolve<IMediator>();
            return mediator;
        }

        private Assembly ThisAssembly { get; } = typeof(MediatorBootstrapper).Assembly;

        private IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            RegisterMediatorPipeline(builder);

            builder.RegisterModule<SantaAutofacModule>();

            // Logging
            //builder.RegisterModule<NLogModule>();
            builder
                .RegisterType<ConsoleLogger>()
                .As<IConsoleLogger>();

            // RavenDB
            builder
                .RegisterInstance(RavenDbStore.Initialize())
                .As<IDocumentStore>()
                .SingleInstance();

            var container = builder.Build();

            // The below returns:
            //  - RequestPreProcessorBehavior
            //  - RequestPostProcessorBehavior
            //  - GenericPipelineBehavior

            //var behaviors = container
            //    .Resolve<IEnumerable<IPipelineBehavior<Ping, Pong>>>()
            //    .ToList();
            return container;
        }

        private void RegisterMediatorPipeline(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestHandler<>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(ThisAssembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            // It appears Autofac returns the last registered types first
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestLoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestTransctionBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });
        }
    }
}
