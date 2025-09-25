using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Exceptions.Security;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System.Security.Cryptography.X509Certificates;

namespace ECommerce.Shared.Infrastructure.RavenDB;

public static class RegisterRaven
{
    public static void AddRavenDB(this IServiceCollection services)
    {
        var options = new RavenSettings();
        var config = services.BuildServiceProvider().GetService<IConfiguration>();
        config?.GetSection(RavenSettings.SectionName).Bind(options);

        services.AddSingleton<Lazy<IDocumentStore>, Lazy<IDocumentStore>>(_ => new Lazy<IDocumentStore>(() =>
        {
            var servers = options.Urls.Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();

            var store = new DocumentStore
            {
                Urls = servers,
                Database = options.Databases[0]
            };

            if (!string.IsNullOrWhiteSpace(options.Certificate))
            {
                var pathCertificate = Path.GetFullPath($"{options.Certificate}");

                if (!File.Exists(pathCertificate))
                {
                    throw new CertificateNameMismatchException("Certificate file not exists.");
                }

                store.Certificate = !string.IsNullOrWhiteSpace(options.Password) 
                    ? X509CertificateLoader.LoadCertificateFromFile(pathCertificate)
                    : null;
            }

            store.Initialize();

            foreach (var database in options.Databases)
            {
                var databaseRecord = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(database));

                if (databaseRecord is null)
                {
                    store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));
                }
            }

            return store;
        }));

        services.AddTransient<IRavenDocumentStoreHolder, RavenDocumentStoreHolder>();
    }
}