using Common.Repositories;
using IDM.Application.Abstractions.Services;
using IDM.Application.Abstractions.Synchronization;
using IDM.Application.Repositories;
using IDM.Application.Services.Synchronization;
using IDM.Application.Synchronization;
using IDM.Application.Synchronization.Departments;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Infrastructure.Jobs;
using IDM.Infrastructure.Repositories;
using IDM.Infrastructure.LoadingServices;
using IDM.Infrastructure.Services;
using IDM.Infrastructure.Synchronization;
using IDM.Infrastructure.Synchronization.Departments.Normalization;
using Microsoft.EntityFrameworkCore;
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
        services.AddScoped<IIdmDepartmentNormalizer, IdmDepartmentNormalizer>();
        services.AddScoped<INormalizationProvider, NormalizationProvider>();
        services.AddScoped<IDepartmentNameNormalizer, DepartmentNameNormalizer>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<INormalizationRepository, NormalizationRepository>();
        services.AddScoped<IIdmSynchronizationService, IdmSynchronizationService>();
        //services.AddScoped<,>()

        services.AddScoped<IDepartmentSyncService, DepartmentSyncService>();

        services.AddScoped<IIdmGuidConverter, IdmGuidConverter>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<DbContext>(provider =>
            provider.GetRequiredService<AppDbContext>());
        services.AddScoped<IdmLoadingJob>();
        return services;
    }
}