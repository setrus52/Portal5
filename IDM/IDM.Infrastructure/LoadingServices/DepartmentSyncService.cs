using Common.Repositories;
using IDM.Application.Abstractions.Synchronization;
using IDM.Application.DTO;
using IDM.Application.Repositories;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Records;
using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IDM.Infrastructure.LoadingServices;

public class DepartmentSyncService(
    ILogger<DepartmentSyncService> logger,
    IIdmLoadingService idmLoadingService,
    IIdmNormalizer<DepartmentDto, ExtDepartmentDto> departmentNormalizer,
    IDepartmentRepository departmentRepository)
    : ISyncService<Department>
{
    private readonly ILogger<DepartmentSyncService> _logger = logger;
    private readonly IIdmLoadingService _idmLoadingService = idmLoadingService;
    private readonly IIdmNormalizer<DepartmentDto, ExtDepartmentDto> _departmentNormalizer = departmentNormalizer;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;


    public async Task<List<Department>> SyncAsync(CancellationToken cancellationToken = default)
{
    _logger.LogInformation("Синхронизация отделов начата");

    try
    {
        // 1. Загружаем отделы из IDM
        var extDepartments = await _idmLoadingService
            .LoadDepartmentsAsync(cancellationToken);

        // 2. Нормализуем
        var departments = await _departmentNormalizer
            .NormalizeAsync(extDepartments, cancellationToken);

        // 3. Загружаем существующие отделы
        var dbDepartmentsByGuid = await _departmentRepository
            .QueryTracking()
            .Where(x => !x.IsManual)
            .ToDictionaryAsync(x => x.Guid, cancellationToken);

        var added = 0;
        var updated = 0;

        foreach (var dto in departments)
        {
            if (dbDepartmentsByGuid.TryGetValue(dto.Guid, out var department))
            {
                UpdateDepartment(department, dto);
                updated++;
            }
            else
            {
                department = CreateDepartment(dto);

                _departmentRepository.Add(department);

                // Добавляем в словарь, чтобы итоговая коллекция была актуальной
                dbDepartmentsByGuid.Add(department.Guid, department);

                added++;
            }
        }

        _logger.LogInformation(
            "Отделы. Добавлено: {Added}, обновлено: {Updated}",
            added,
            updated);

        // 4. Деактивируем отсутствующие в IDM отделы
        var actualDepartmentGuids = departments
            .Select(x => x.Guid)
            .ToHashSet();

        foreach (var department in dbDepartmentsByGuid.Values)
        {
            if (!actualDepartmentGuids.Contains(department.Guid))
                department.IsActual = false;
        }

        _logger.LogInformation(
            "Синхронизация отделов завершена. Получено из IDM: {Count}",
            departments.Count);

        return dbDepartmentsByGuid.Values.ToList();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Ошибка при синхронизации отделов");
        throw;
    }
}


    private static Department CreateDepartment(DepartmentDto dto)
        => new()
        {
            Guid = dto.Guid,

            Name = dto.Name,
            SourceName = dto.SourceName,
            ShortName = dto.ShortName,
            ParentGuid = dto.ParentGuid,
            //SupervisorGuid = dto.SupervisorGuid,

            IsActual = dto.IsActual,

            // Новый отдел пришел из IDM,
            // поэтому он автоматический
            IsManual = false,

            // Родитель будет определяться автоматически
            IsParentDepartmentDefinedManually = false
        };


    private static void UpdateDepartment(Department department, DepartmentDto dto)
    {
        // Ручные отделы не изменяем вообще
        if (department.IsManual)
            return;

        department.Name = dto.Name;
        department.SourceName = dto.SourceName;
        department.ShortName = dto.ShortName;

        //department.SupervisorGuid = dto.SupervisorGuid;

        department.IsActual = dto.IsActual;

        if (!department.IsParentDepartmentDefinedManually) department.ParentGuid = dto.ParentGuid;
        // ParentGuid здесь НЕ обновляем.
        // Это ответственность DepartmentTreeUpdater.
    }
}