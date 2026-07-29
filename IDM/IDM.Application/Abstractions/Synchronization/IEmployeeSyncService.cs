using IDM.Domain.Entities;

namespace IDM.Application.Abstractions.Synchronization;

public interface IEmployeeSyncService
{
    Task<List<Employee>> SyncAsync(CancellationToken cancellationToken = default);
}