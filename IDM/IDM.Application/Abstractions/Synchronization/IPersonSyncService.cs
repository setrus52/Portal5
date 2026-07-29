using IDM.Domain.Entities;

namespace IDM.Application.Abstractions.Synchronization;

public interface IPersonSyncService
{
    Task<List<Person>> SyncAsync(CancellationToken cancellationToken = default);
}