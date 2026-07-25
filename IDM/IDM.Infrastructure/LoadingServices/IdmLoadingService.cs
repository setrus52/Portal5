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
    Task<List<ExtDepartmentDto>> LoadDepartmentsAsync();
    Task<List<ExtPersonDto>> LoadPersonsAsync();
    Task<List<ExtPositionDto>> LoadPositionsAsync();
    Task<List<ExtEmployeeDto>> LoadEmployeesAsync();
    Task<List<ExtAbsenceDto>> LoadAbsencesAsync();
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


    public Task<List<ExtDepartmentDto>> LoadDepartmentsAsync()
        => LoadAsync<ExtDepartmentDto>("Departments");

    public Task<List<ExtPersonDto>> LoadPersonsAsync()
        => LoadAsync<ExtPersonDto>("Persons");

    public Task<List<ExtPositionDto>> LoadPositionsAsync()
        => LoadAsync<ExtPositionDto>("Positions");

    public Task<List<ExtEmployeeDto>> LoadEmployeesAsync()
        => LoadAsync<ExtEmployeeDto>("Employees");

    public Task<List<ExtAbsenceDto>> LoadAbsencesAsync()
        => LoadAsync<ExtAbsenceDto>("Absences");


    private string GetUrl(string name)
    {
        var point = _idm.Points.FirstOrDefault(x => x.Name == name)
                    ?? throw new InvalidOperationException(
                        $"В конфигурации IDM (appsettings.json) не найден endpoint '{name}'.");

        return _idm.Root.AppendPathSegment(point.Point);
    }

    private async Task<List<T>> LoadAsync<T>(string endpointName)
    {
        var url = GetUrl(endpointName);

        try
        {
            _logger.LogInformation("Начата загрузка '{Endpoint}'. Url: {Url}", endpointName, url);

            return await url.GetJsonAsync<List<T>>();
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