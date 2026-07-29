using IDM.Application.Abstractions.Synchronization;
using IDM.Domain.Entities;

namespace IDM.Infrastructure.LoadingServices;

public class EmployeeSyncService : IEmployeeSyncService
{
    public Task<List<Employee>> SyncAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}