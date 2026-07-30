using Common.Repositories;
using IDM.Application.Abstractions.Services;
using IDM.Application.Abstractions.Synchronization;
using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IDM.Application.Services.Synchronization;

public class IdmSynchronizationService(
    ILogger<IdmSynchronizationService> logger,
    ISyncService<Department> departmentSyncService,
    ISyncService<Position> positionSyncService,
    ISyncService<Person> personSyncService,
    ISyncService<Employee> employeeSyncService,
    //IDepartmentTreeUpdater departmentTreeUpdater,
    //ISupervisorSyncService supervisorSyncService,
    //IAbsenceSyncService absenceSyncService 
    //IPhonebookService phonebookService 
    IUnitOfWork unitOfWork)
    : IIdmSynchronizationService
{
    private readonly ILogger<IdmSynchronizationService> _logger = logger;

    public async Task SynchronizeAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(
                "Полная синхронизация IDM начата");


            var departments = await departmentSyncService
                .SyncAsync(cancellationToken);

            var positions = await positionSyncService
                .SyncAsync(cancellationToken);

            var persons = await personSyncService.SyncAsync(cancellationToken);

            var employees = await employeeSyncService.SyncAsync(cancellationToken);

            var count = await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("EF сохранил изменений: {Count}", count);

            _logger.LogInformation(
                "Полная синхронизация IDM завершена");
        }
        catch (DbUpdateException ex)
        {
            foreach (var entry in ex.Entries)
            {
                _logger.LogError(
                    "Ошибка сохранения сущности {Entity}",
                    entry.Entity.GetType().Name);
                if (ex.InnerException != null)
                    _logger.LogError(
                        "InnerException: {Message}",
                        ex.InnerException.Message);
            }

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Ошибка полной синхронизации IDM");

            throw;
        }
    }
}
/*public async Task SynchronizeAsync(CancellationToken contextCancellationToken)
{
    await departmentSyncService.SyncAsync(contextCancellationToken);
    //+await _personSync.SyncAsync();
    //+await _positionSync.SyncAsync();
    //+await _employeeSync.SyncAsync();
    //+await _supervisorSync.SyncAsync();
    //+await _absenceSync.SyncAsync();
    //await _phonebookBuilder.BuildAsync();
}*/