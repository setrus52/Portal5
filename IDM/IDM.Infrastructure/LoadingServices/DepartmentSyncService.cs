using Common.Enums;
using IDM.Application.Abstractions.Synchronization;
using IDM.Application.DTO;
using IDM.Application.Repositories;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Departments.Tree;
using IDM.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace IDM.Infrastructure.LoadingServices;

public class DepartmentSyncService : IDepartmentSyncService
{
    private readonly ILogger<DepartmentSyncService> _logger;
    private readonly IIdmLoadingService _idmLoadingService;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDepartmentNameNormalizer _nameNormalizer;
    private readonly IDepartmentTreeUpdater _treeUpdater;

    public DepartmentSyncService(
        ILogger<DepartmentSyncService> logger,
        IIdmLoadingService idmLoadingService,
        IDepartmentRepository departmentRepository,
        IDepartmentNameNormalizer nameNormalizer,
        IDepartmentTreeUpdater treeUpdater)
    {
        _logger = logger;
        _idmLoadingService = idmLoadingService;
        _departmentRepository = departmentRepository;
        _nameNormalizer = nameNormalizer;
        _treeUpdater = treeUpdater;
    }

    public async Task SyncAsync()
    {
        try
        {
            _logger.LogInformation("Синхронизация отделов начата");

            // ВОТ ЗДЕСЬ - просто загружаем нормализации из БД каждый раз
            var normalizations = await _departmentRepository.GetByCategoryAsync(NormalizationCategory.DepartmentName);
            var replacements = normalizations.ToDictionary(n => n.SearchText, n => n.ReplacementText);
            
            var idmDepartments = await _idmLoadingService.LoadDepartmentsAsync();
            
            // Применяем замены
            var normalizedDepartments = idmDepartments
                .Select(d => d.ToDto())
                .Select(d => ApplyReplacements(d, replacements))
                .ToList();

            var portalDepartments = await _departmentRepository.GetAllAsync();

            await RemoveNonExistentAsync(normalizedDepartments, portalDepartments);
            await _treeUpdater.UpdateTreeAsync(normalizedDepartments, portalDepartments);

            _logger.LogInformation($"Синхронизация отделов завершена. Обновлено: {normalizedDepartments.Count}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при синхронизации отделов");
            throw;
        }
    }

    private DepartmentDto ApplyReplacements(DepartmentDto department, Dictionary<string, string> replacements)
    {
        var name = department.name;
        var shortName = department.shortName;

        foreach (var (search, replace) in replacements)
        {
            name = name.Replace(search, replace);
            if (shortName != null)
                shortName = shortName.Replace(search, replace);
        }

        return department with { name = name, shortName = shortName };
    }
    
    private async Task RemoveNonExistentAsync(
        IEnumerable<DepartmentDto> idmDepartments,
        IEnumerable<Department> portalDepartments)
    {
        var idmGuids = idmDepartments.Select(p => p.guid).ToHashSet();
        var toRemove = portalDepartments
            .Where(p => !p.IsManual && !idmGuids.Contains(p.Guid.ToString()))
            .ToList();

        if (toRemove.Any())
        {
            await _departmentRepository.RemoveRangeAsync(toRemove);
            _logger.LogInformation($"Удалено отделов: {toRemove.Count}");
        }
    }
}