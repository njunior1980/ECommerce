using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;
using Raven.Client.Documents;

namespace ECommerce.Catalog.Commands;

public record UpdateCategoryCommand(string Id, string Name) : ICommand<Result>;

public class UpdateCategoryHandler(IRavenDocumentStoreHolder storeHolder) : ICommandHandler<UpdateCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken ct = default)
    {
        using (var session = storeHolder.OpenSession(Constants.DatabaseName))
        {
            try
            {
                var category = await session.LoadAsync<Domain.Category>(command.Id, ct);

                if (category is null)
                {
                    return Result.Failure(Error.NotFound("Category.Update", $"Category with ID '{command.Id}' was not found."));
                }

                if (await session.Query<Domain.Category>().AnyAsync(c => c.Name.ToLower().Trim() == command.Name.ToLower().Trim() && c.Id != command.Id, ct))
                {
                    return Result.Failure(Error.Failure("Category.Update", "Another category with the same name already exists."));
                }

                category.Update(command.Name);

                await session.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(Error.Exception(e.Message));
            }
        }
    }
}