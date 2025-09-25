using Microsoft.AspNetCore.Routing;

namespace ECommerce.Shared.Core.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder builder);
}