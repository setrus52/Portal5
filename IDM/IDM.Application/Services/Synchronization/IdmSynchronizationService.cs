using Common.Repositories;
using IDM.Application.Abstractions.Services;
using IDM.Application.Abstractions.Synchronization;
using Microsoft.Extensions.Logging;

namespace IDM.Application.Services.Synchronization;

public class IdmSynchronizationService(
    ILogger<IdmSynchronizationService> logger,
    IDepartmentSyncService departmentSyncService,
    //IPersonSyncService personSyncService,
    //IEmployeeSyncService employeeSyncService,
    //IPositionSyncService positionSyncService,
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


            await departmentSyncService
                .SyncAsync(cancellationToken);


            /*await employeeSyncService
                .SyncAsync(cancellationToken);


            await positionSyncService
                .SyncAsync(cancellationToken);


            await departmentTreeUpdater
                .UpdateAsync(cancellationToken);*/

            // УДАЛИТЬ!!!
            _logger.LogInformation(
                "DepartmentSyncService завершен, вызываю SaveChanges");

            var count = await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("EF сохранил изменений: {Count}", count);

            await unitOfWork
                .SaveChangesAsync(cancellationToken);


            _logger.LogInformation(
                "Полная синхронизация IDM завершена");
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