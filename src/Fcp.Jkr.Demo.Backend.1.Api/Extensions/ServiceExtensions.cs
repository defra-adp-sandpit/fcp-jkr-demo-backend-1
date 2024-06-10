using Fcp.Jkr.Demo.Backend.1.Core.Interfaces;
using Fcp.Jkr.Demo.Backend.1.Core.Services;

namespace Fcp.Jkr.Demo.Backend.1.Api.Extensions;
public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IItemService, ItemService>();
        return services;
    }
}
