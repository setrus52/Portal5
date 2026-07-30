namespace IDM.Application.Abstractions.Synchronization;

public interface ISyncService<T>
{
    Task<List<T>> SyncAsync(CancellationToken cancellationToken = default);
}