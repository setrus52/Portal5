using IDM.Application.Abstractions.Services;

namespace IDM.Application.Services.Synchronization;

public class IdmSynchronizationService : IIdmSynchronizationService
{
    public async Task SynchronizeAsync()
    {
        await _departmentSync.SyncAsync();
        await _personSync.SyncAsync();
        await _positionSync.SyncAsync();
        await _employeeSync.SyncAsync();
        await _supervisorSync.SyncAsync();
        await _absenceSync.SyncAsync();
        await _phonebookBuilder.BuildAsync();
    }
}