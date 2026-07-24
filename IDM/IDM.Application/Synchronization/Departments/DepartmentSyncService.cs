using IDM.Application.Abstractions.Synchronization;
using IDM.Application.DTO;
using IDM.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace IDM.Application.Synchronization.Departments;

public class DepartmentSyncService<IIdmDataProvider, IDepartmentRepository> : IDepartmentSyncService
{
    private readonly ILogger<DepartmentSyncService> _logger;
    private readonly IIdmDataProvider _dataProvider;
    private readonly IDepartmentRepository _repository;
    private readonly IDepartmentNameNormalizer _nameNormalizer;
    private readonly IDepartmentTreeUpdater _treeUpdater;

    public DepartmentSyncService(
        ILogger<DepartmentSyncService> logger,
        IIdmDataProvider dataProvider,
        IDepartmentRepository repository,
        IDepartmentNameNormalizer nameNormalizer,
        IDepartmentTreeUpdater treeUpdater)
    {
        _logger = logger;
        _dataProvider = dataProvider;
        _repository = repository;
        _nameNormalizer = nameNormalizer;
        _treeUpdater = treeUpdater;
    }

    public async Task SyncAsync()
    {
        try
        {
            _logger.LogInformation("Синхронизация отделов начата");

            // ВОТ ЗДЕСЬ - просто загружаем нормализации из БД каждый раз
            var normalizations = await _repository.GetByCategoryAsync(NormalizationCategory.DepartmentName);
            var replacements = normalizations.ToDictionary(n => n.SearchText, n => n.ReplacementText);
            
            var idmDepartments = await _dataProvider.LoadDepartmentsAsync();
            
            // Применяем замены
            var normalizedDepartments = idmDepartments
                .Select(d => d.ToDto())
                .Select(d => ApplyReplacements(d, replacements))
                .ToList();

            var portalDepartments = await _repository.GetAllAsync();

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
            await _repository.RemoveRangeAsync(toRemove);
            _logger.LogInformation($"Удалено отделов: {toRemove.Count}");
        }
    }
}