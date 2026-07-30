using IDM.Application.Abstractions.Synchronization;
using IDM.Application.DTO;
using IDM.Application.Repositories;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Records;
using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IDM.Infrastructure.LoadingServices;

public class EmployeeSyncService(
    ILogger<EmployeeSyncService> logger,
    IIdmLoadingService idmLoadingService,
    IIdmNormalizer<EmployeeDto, ExtEmployeeDto> employeeNormalizer,
    IEmployeeRepository employeeRepository)
    : ISyncService<Employee>
{
    private readonly ILogger<EmployeeSyncService> _logger = logger;
    private readonly IIdmNormalizer<EmployeeDto, ExtEmployeeDto> _employeeNormalizer = employeeNormalizer;
    private readonly IEmployeeRepository _employeeRepository =  employeeRepository;
    private readonly IIdmLoadingService _idmLoadingService = idmLoadingService;

    public async Task<List<Employee>> SyncAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Синхронизация сотрудников начата");

        try
        {
            // 1. Получаем сотрудников из IDM
            var extEmployees = await _idmLoadingService
                .LoadEmployeesAsync(cancellationToken);

            // 2. Нормализуем
            var employees = await _employeeNormalizer
                .NormalizeAsync(extEmployees, cancellationToken);

            // 3. Получаем существующих сотрудников
            var dbEmployeesByGuid = await _employeeRepository
                .QueryTracking()
                .ToDictionaryAsync(x => x.Guid, cancellationToken);

            var added = 0;
            var updated = 0;

            // 4. Добавляем новые и обновляем существующие
            foreach (var dto in employees)
            {
                if (dbEmployeesByGuid.TryGetValue(dto.Guid, out var employee))
                {
                    UpdateEmployee(employee, dto);
                    updated++;
                }
                else
                {
                    employee = CreateEmployee(dto);

                    _employeeRepository.Add(employee);

                    // Добавляем в словарь, чтобы вернуть актуальный список
                    dbEmployeesByGuid.Add(employee.Guid, employee);

                    added++;
                }
            }

            _logger.LogInformation(
                "Сотрудники. Добавлено: {Added}, обновлено: {Updated}",
                added,
                updated);

            _logger.LogInformation(
                "Синхронизация сотрудников завершена. Получено из IDM: {Count}",
                employees.Count);

            return dbEmployeesByGuid.Values.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при синхронизации сотрудников");
            throw;
        }
    }

    private static Employee CreateEmployee(EmployeeDto dto)
        => new Employee
        {
            Guid = dto.Guid,
            DepartmentGuid =  dto.DepartmentGuid,
            PersonGuid = dto.PersonGuid,
            PositionGuid =  dto.PositionGuid,
            IsMain = dto.IsMain,
            IsActual =  dto.IsActual,
            EmploymentDate =  dto.EmploymentDate,
            DismissalDate = dto.DismissalDate,
            NextPlannedVacationDate = dto.NextPlannedVacationDate,
            VacationRemainingDays = dto.VacationRemainingDays
        };
    
    private static void UpdateEmployee(Employee employee, EmployeeDto dto)
    {
        employee.DepartmentGuid = dto.DepartmentGuid;
        employee.PersonGuid = dto.PersonGuid;
        employee.PositionGuid = dto.PositionGuid;

        employee.IsMain = dto.IsMain;
        employee.IsActual = dto.IsActual;

        employee.EmploymentDate = dto.EmploymentDate;
        employee.DismissalDate = dto.DismissalDate;
        employee.NextPlannedVacationDate = dto.NextPlannedVacationDate;
        employee.VacationRemainingDays = dto.VacationRemainingDays;
    }
}