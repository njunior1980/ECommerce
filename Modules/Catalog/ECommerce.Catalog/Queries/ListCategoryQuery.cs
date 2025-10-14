using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;

namespace ECommerce.Catalog.Queries;

public record CategoryResult(string Id, string Name);

public record ListCategoryQuery(string SearchTerm, int PageNumber = 1, int PageSize = 10) : IQuery<Result<IReadOnlyList<CategoryResult>>>;

public class ListCategoryHandler(IRavenDocumentStoreHolder storeHolder) : IQueryHandler<ListCategoryQuery, Result<IReadOnlyList<CategoryResult>>>
{
    public async Task<Result<IReadOnlyList<CategoryResult>>> Handle(ListCategoryQuery query, CancellationToken ct = default)
    {
        using (var session = storeHolder.OpenSession(Constants.DatabaseName))
        {
            var categoriesQuery = await session.Advanced.AsyncDocumentQuery<Domain.Category>()
                .Search(c => c.Name, $"*{query.SearchTerm}*")
                .OrderBy(c => c.Name)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(ct);

            var categoryResults = categoriesQuery.Select(c => new CategoryResult(c.Id, c.Name)).ToList();
            return Result.Success<IReadOnlyList<CategoryResult>>(categoryResults);
        }
    }
}