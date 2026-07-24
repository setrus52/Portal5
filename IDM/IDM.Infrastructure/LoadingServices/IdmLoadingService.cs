using Common.Enums;
using Flurl.Http;
using IDM.Infrastructure.Options.EndpointOptions;
using Microsoft.Extensions.Options;

namespace IDM.Infrastructure.LoadingServices;

#region IDM DTO transport resords

public record DepartmentsDto(
    string guid,
    string name,
    string? shortName,
    string? parent,
    string? supervisorGuid,
    string type,
    bool isActual);

public record PersonDto(
    string guid,
    string code,
    string surname,
    string name,
    string patronymic,
    Gender gender,
    DateTime birthday,
    string login,
    string domain,
    string email,
    string officeNumber,
    string internalPhone,
    string mobilePhone,
    string photo);

public record PositionDto(
    string guid,
    string name,
    int orderPos);

public record EmployeeDto(
    string employeeGuid,
    string personGuid,
    string departmentGuid,
    string positionGuid,
    bool isMain,
    bool isActual,
    DateTime employmentDate,
    DateTime? dismissalDate,
    DateTime? nextPlannedVacationDate,
    decimal? remainingDayCount);

public record AbsenceDto(
    string guid,
    string employeeGuid,
    DateTime startDate,
    DateTime endDate,
    string reason,
    string description
);

#endregion

public interface IIdmLoadingService
{
    Task<List<DepartmentsDto>> LoadDepartments();
    Task<List<PersonDto>> LoadPersons();
    Task<List<PositionDto>> LoadPositions();
    Task<List<EmployeeDto>> LoadEmployees();
    Task<List<AbsenceDto>> LoadAbsences();
}

public class IdmLoadingService : IIdmLoadingService
{
    private readonly string _baseUrl = $"http://idm.yuresk.local/api/";
    private readonly EndpointGroup _idm;

    public IdmLoadingService(IOptions<EndpointOptions> options)
    {
        _idm = options.Value.Groups
            .First(x => x.Code == "Idm");
    }


    public Task<List<DepartmentsDto>> LoadDepartments()
        => LoadAsync<DepartmentsDto>("Departments");

    public Task<List<PersonDto>> LoadPersons()
        => LoadAsync<PersonDto>("Persons");

    public Task<List<PositionDto>> LoadPositions()
        => LoadAsync<PositionDto>("Positions");

    public Task<List<EmployeeDto>> LoadEmployees()
        => LoadAsync<EmployeeDto>("Employees");

    public Task<List<AbsenceDto>> LoadAbsences()
        => LoadAsync<AbsenceDto>("Absences");


    private string GetUrl(string name)
    {
        var point = _idm.Points.First(x => x.Name == name);
        return $"{_idm.Root}{point.Point}";
    }

    private Task<List<T>> LoadAsync<T>(string endpointName)
    {
        var url = GetUrl(endpointName);
        return url.GetJsonAsync<List<T>>();
    }
}