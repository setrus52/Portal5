using IDM.Domain.Entities;

namespace IDM.Application.Abstractions.Synchronization;

public interface IDepartmentSyncService
{
    Task<List<Department>> SyncAsync(CancellationToken cancellationToken = default);
}