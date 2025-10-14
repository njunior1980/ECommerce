using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;
using Raven.Client.Documents;

namespace ECommerce.Catalog.Commands;

public record CreateCategoryCommand(string Name) : ICommand<Result<string>>;

public class CreateCategoryHandler(IRavenDocumentStoreHolder storeHolder) : ICommandHandler<CreateCategoryCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCategoryCommand command, CancellationToken ct = default)
    {
        using (var session = storeHolder.OpenSession(Constants.DatabaseName))
        {
            try
            {
                if (await session.Query<Domain.Category>().AnyAsync(c => c.Name.ToLower().Trim() == command.Name.ToLower().Trim(), ct))
                {
                    return Result.Failure<string>(Error.Failure("Category.Create","Category with the same name already exists."));
                }

                var category = Domain.Category.Create(command.Name);

                await session.StoreAsync(category, category.Id, ct);
                await session.SaveChangesAsync(ct);

                return Result.Success(category.Id);
            }
            catch (Exception e)
            {
                return Result.Failure<string>(Error.Exception(e.Message));
            }
        }
    }
}