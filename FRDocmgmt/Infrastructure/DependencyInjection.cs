using FRDocmgmt.Application.Documents;
using FRDocmgmt.Infrastructure.Documents;
using Microsoft.Extensions.DependencyInjection;

namespace FRDocmgmt.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDocumentRepository, InMemoryDocumentRepository>();
        return services;
    }
}
