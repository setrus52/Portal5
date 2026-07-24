using IDM.Application.Abstractions.Services;
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

        try
        {
            await _service.SynchronizeAsync(context.CancellationToken);
            _logger.LogInformation($"{nameof(IdmLoadingJob)} завершен успешно");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(IdmLoadingJob)} завершен с ошибкой");
            throw;
        }
    }
}