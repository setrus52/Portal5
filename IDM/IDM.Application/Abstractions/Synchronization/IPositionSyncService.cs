using IDM.Domain.Entities;

namespace IDM.Application.Abstractions.Synchronization;

public interface IPositionSyncService
{
    Task<List<Position>> SyncAsync(CancellationToken cancellationToken = default);
}