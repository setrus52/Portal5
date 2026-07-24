namespace IDM.Application.Abstractions.Services;

public interface IIdmSynchronizationService
{
    Task SynchronizeAsync(CancellationToken contextCancellationToken);
}