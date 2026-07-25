using IDM.Application.Abstractions.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace IDM.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class IdmLoadingJob(
    ILogger<IdmLoadingJob> logger,
    IIdmSynchronizationService service)
    : IJob
{
    public async Task Execute(
        IJobExecutionContext context)
    {
        logger.LogInformation("{Job} запущен", nameof(IdmLoadingJob));

        try
        {
            await service
                .SynchronizeAsync(context.CancellationToken);


            logger.LogInformation("{Job} завершен успешно", nameof(IdmLoadingJob));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Job} завершен с ошибкой", nameof(IdmLoadingJob));

            throw;
        }
    }
}