using System.Reflection;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database.Server;

namespace IzzyDevo.CQRS.Infrastructure.Database
{
    public static class RavenDbStore
    {
        public static int DefaultPort { get; } = 8888;

        public static IDocumentStore Initialize()
        {
            var store = new EmbeddableDocumentStore
            {
                DataDirectory = "Data",
                UseEmbeddedHttpServer = true,
                //RunInMemory = true,
                Configuration =
                {
                    Port = DefaultPort,
                    //HostName = "localhost"
                    AnonymousUserAccessMode = AnonymousUserAccessMode.All
                },
            };

            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(DefaultPort);

            store.Conventions.IdentityPartsSeparator = "-";
            store.Initialize();

            IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), store);

            return store;
        }
    }
}
