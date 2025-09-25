using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ECommerce.Shared.Infrastructure.RavenDB;

public interface IRavenDocumentStoreHolder
{
    IDocumentStore Store { get; }
    IAsyncDocumentSession OpenSession(string database);
}