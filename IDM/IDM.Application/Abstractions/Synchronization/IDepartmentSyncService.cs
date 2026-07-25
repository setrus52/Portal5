namespace IDM.Application.Abstractions.Synchronization;

public interface IDepartmentSyncService
{
    Task SyncAsync(CancellationToken cancellationToken = default);
}