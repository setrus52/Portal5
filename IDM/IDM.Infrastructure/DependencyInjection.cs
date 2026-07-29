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
        /*services.AddScoped<IIdmLoadingService, IdmLoadingService>();
        services.AddScoped<IIdmDepartmentNormalizer, IdmDepartmentNormalizer>();
        services.AddScoped<INormalizationProvider, NormalizationProvider>();
        services.AddScoped<IDepartmentNameNormalizer, DepartmentNameNormalizer>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<INormalizationRepository, NormalizationRepository>();
        services.AddScoped<IIdmSynchronizationService, IdmSynchronizationService>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<IPositionSyncService, PositionSyncService>();
        services.AddScoped<IIdmPositionNormalizer, IdmPositionNormalizer>();
        services.AddScoped<IPositionNameNormalizer, PositionNameNormalizer>();
        services.AddScoped<IDepartmentSyncService, DepartmentSyncService>();

        services.AddScoped<IIdmGuidConverter, IdmGuidConverter>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<DbContext>(provider =>
            provider.GetRequiredService<AppDbContext>());
        services.AddScoped<IdmLoadingJob>();*/

        // DbContext
        services.AddDatabase();

        // IDM
        services.AddIdm();

        // Репозитории
        services.AddRepositories();

        // Нормализация
        services.AddNormalization();

        // Сервисы синхронизации
        services.AddSynchronization();

        // Quartz
        services.AddJobs();

        return services;
    }

    #region DbContext

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<DbContext>(p => p.GetRequiredService<AppDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    #endregion

    #region Репозитории

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<INormalizationRepository, NormalizationRepository>();

        return services;
    }

    #endregion

    #region Нормализация

    private static IServiceCollection AddNormalization(this IServiceCollection services)
    {
        services.AddScoped<INormalizationProvider, NormalizationProvider>();

        services.AddScoped<IIdmDepartmentNormalizer, IdmDepartmentNormalizer>();
        services.AddScoped<IDepartmentNameNormalizer, DepartmentNameNormalizer>();

        services.AddScoped<IIdmPositionNormalizer, IdmPositionNormalizer>();
        services.AddScoped<IPositionNameNormalizer, PositionNameNormalizer>();

        services.AddScoped<IIdmPersonNormalizer, IdmPersonNormalizer>();

        return services;
    }

    #endregion

    #region IDM

    private static IServiceCollection AddIdm(this IServiceCollection services)
    {
        services.AddScoped<IIdmLoadingService, IdmLoadingService>();
        services.AddScoped<IIdmGuidConverter, IdmGuidConverter>();

        return services;
    }

    #endregion

    #region Сервисы синхронизации

    private static IServiceCollection AddSynchronization(this IServiceCollection services)
    {
        services.AddScoped<IIdmSynchronizationService, IdmSynchronizationService>();

        services.AddScoped<IDepartmentSyncService, DepartmentSyncService>();
        services.AddScoped<IPositionSyncService, PositionSyncService>();

        services.AddScoped<IPersonSyncService, PersonSyncService>();
        services.AddScoped<IEmployeeSyncService, EmployeeSyncService>();


        return services;
    }

    #endregion

    #region Quartz

    private static IServiceCollection AddJobs(this IServiceCollection services)
    {
        services.AddScoped<IdmLoadingJob>();

        return services;
    }

    #endregion
}