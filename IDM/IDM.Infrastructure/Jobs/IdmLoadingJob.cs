using Microsoft.Extensions.Logging;
using Quartz;

namespace IDM.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class IdmLoadingJob : IJob
{
    private readonly ILogger<IdmLoadingJob> _logger;
    private readonly IIdmSynchronizationService _service;

    public IdmLoadingJob(ILogger<IdmLoadingJob> logger, IIdmSynchronizationService service)
    {
        _logger = logger;
        _service = service;
    }


    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"{nameof(IdmLoadingJob)} запущен...");
        await _service.SynchronizeAsync(context.CancellationToken);
    }
}