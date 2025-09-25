using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ECommerce.Shared.Infrastructure.RavenDB;

internal class RavenDocumentStoreHolder(Lazy<IDocumentStore> documentStore) : IRavenDocumentStoreHolder
{
    public IDocumentStore Store => documentStore.Value;

    public IAsyncDocumentSession OpenSession(string database)
        => documentStore.Value.OpenAsyncSession(database);
}