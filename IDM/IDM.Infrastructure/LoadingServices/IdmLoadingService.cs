using Common.Enums;
using Flurl;
using Flurl.Http;
using IDM.Application.Synchronization.Records;
using IDM.Infrastructure.Options.EndpointOptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IDM.Infrastructure.LoadingServices;

public interface IIdmLoadingService
{
    Task<List<ExtDepartmentDto>> LoadDepartmentsAsync(CancellationToken cancellationToken = default);
    Task<List<ExtPersonDto>> LoadPersonsAsync(CancellationToken cancellationToken = default);
    Task<List<ExtPositionDto>> LoadPositionsAsync(CancellationToken cancellationToken = default);
    Task<List<ExtEmployeeDto>> LoadEmployeesAsync(CancellationToken cancellationToken = default);
    Task<List<ExtAbsenceDto>> LoadAbsencesAsync(CancellationToken cancellationToken = default);
}

public class IdmLoadingService : IIdmLoadingService
{
    private readonly EndpointGroup _idm;
    private readonly ILogger<IdmLoadingService> _logger;

    public IdmLoadingService(ILogger<IdmLoadingService> logger, IOptions<EndpointOptions> options)
    {
        _idm = options.Value.Groups.FirstOrDefault(x => x.Code == "Idm")
               ?? throw new InvalidOperationException(
                   "В appsettings.json отсутствует группа endpoint'ов с Code = 'Idm'.");
        _logger = logger;
    }


    public Task<List<ExtDepartmentDto>> LoadDepartmentsAsync(CancellationToken cancellationToken = default)
        => LoadAsync<ExtDepartmentDto>("Departments", cancellationToken);

    public Task<List<ExtPersonDto>> LoadPersonsAsync(CancellationToken cancellationToken = default)
        => LoadAsync<ExtPersonDto>("Persons", cancellationToken);

    public Task<List<ExtPositionDto>> LoadPositionsAsync(CancellationToken cancellationToken = default)
        => LoadAsync<ExtPositionDto>("Positions", cancellationToken);

    public Task<List<ExtEmployeeDto>> LoadEmployeesAsync(CancellationToken cancellationToken = default)
        => LoadAsync<ExtEmployeeDto>("Employees", cancellationToken);

    public Task<List<ExtAbsenceDto>> LoadAbsencesAsync(CancellationToken cancellationToken = default)
        => LoadAsync<ExtAbsenceDto>("Absences", cancellationToken);


    private string GetUrl(string name)
    {
        var point = _idm.Points.FirstOrDefault(x => x.Name == name)
                    ?? throw new InvalidOperationException(
                        $"В конфигурации IDM (appsettings.json) не найден endpoint '{name}'.");

        return _idm.Root.AppendPathSegment(point.Point);
    }

    private async Task<List<T>> LoadAsync<T>(string endpointName, CancellationToken cancellationToken = default)
    {
        var url = GetUrl(endpointName);

        try
        {
            _logger.LogInformation("Начата загрузка '{Endpoint}'. Url: {Url}", endpointName, url);

            return await url.GetJsonAsync<List<T>>(cancellationToken: cancellationToken);
        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError(ex, "HTTP-ошибка при обращении к '{Endpoint}'. Url: {Url}", endpointName, url);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при обращении к '{Endpoint}'. Url: {Url}", endpointName, url);
            throw;
        }
    }
}