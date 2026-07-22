using IDM.Infrastructure.Repositories;
using IDM.Infrastructure.LoadingServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IDM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IIdmLoadingService, IdmLoadingService>();

        services.AddScoped<IDepartmentsRepository, DepartmentRepository>();
        //services.AddScoped<IDepartmentTreeService>()

        return services;
    }
}