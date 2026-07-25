using Common.Repositories;
using IDM.Application.Abstractions.Synchronization;
using IDM.Application.DTO;
using IDM.Application.Repositories;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IDM.Infrastructure.LoadingServices;

public class DepartmentSyncService(
    ILogger<DepartmentSyncService> logger,
    IIdmLoadingService idmLoadingService,
    IIdmDepartmentNormalizer departmentNormalizer,
    IDepartmentRepository departmentRepository)
    : IDepartmentSyncService
{
    private readonly ILogger<DepartmentSyncService> _logger = logger;
    private readonly IIdmLoadingService _idmLoadingService = idmLoadingService;
    private readonly IIdmDepartmentNormalizer _departmentNormalizer = departmentNormalizer;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;


    public async Task SyncAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Синхронизация отделов начата");


            // 1. Получаем отделы из IDM
            var extDepartments = await _idmLoadingService
                .LoadDepartmentsAsync();


            // 2. Нормализуем данные IDM:
            //    string guid -> Guid
            //    string name -> нормализованное имя
            var departments = await _departmentNormalizer
                .NormalizeAsync(extDepartments, cancellationToken);


            // 3. Получаем существующие отделы из БД
            var dbDepartments = await _departmentRepository
                .QueryTracking()
                .ToListAsync(cancellationToken);


            var dbDepartmentsByGuid = dbDepartments
                .ToDictionary(x => x.Guid);


            // 4. Добавляем новые и обновляем существующие
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
                    _departmentRepository.Add(CreateDepartment(dto));
                    added++;
                }
            }

            _logger.LogInformation("Отделы. Добавлено: {Added}, обновлено: {Updated}", added, updated);

            // 5. Деактивируем отделы, которых больше нет в IDM
            var actualDepartmentGuids = departments
                .Select(x => x.Guid)
                .ToHashSet();


            foreach (var department in dbDepartments)
            {
                if (department.IsManual)
                    continue;

                if (!actualDepartmentGuids.Contains(department.Guid)) department.IsActual = false;
            }


            _logger.LogInformation(
                "Синхронизация отделов завершена. Получено из IDM: {Count}",
                departments.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при синхронизации отделов");
            throw;
        }
    }


    private static Department CreateDepartment(DepartmentDto dto) =>
        new()
        {
            Guid = dto.Guid,

            Name = dto.Name,
            SourceName = dto.SourceName,
            ShortName = dto.ShortName,

            //SupervisorGuid = dto.SupervisorGuid,

            IsActual = dto.IsActual,

            // Новый отдел пришел из IDM,
            // поэтому он автоматический
            IsManual = false,

            // Родитель будет определяться автоматически
            IsParentDepartmentDefinedManually = false
        };


    private static void UpdateDepartment(
        Department department,
        DepartmentDto dto)
    {
        // Ручные отделы не изменяем вообще
        if (department.IsManual)
            return;


        department.Name = dto.Name;
        department.SourceName = dto.SourceName;
        department.ShortName = dto.ShortName;

        //department.SupervisorGuid = dto.SupervisorGuid;

        department.IsActual = dto.IsActual;

        // ParentGuid здесь НЕ обновляем.
        // Это ответственность DepartmentTreeUpdater.
    }
}