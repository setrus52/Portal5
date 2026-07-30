using IDM.Application.DTO;
using IDM.Application.Synchronization;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Records;

namespace IDM.Infrastructure.Services;

public class IdmEmployeeNormalizer(
    IIdmGuidConverter guidConverter) 
    : IIdmNormalizer<EmployeeDto, ExtEmployeeDto>
{
    private readonly IIdmGuidConverter _guidConverter = guidConverter;
    public async Task<List<EmployeeDto>> NormalizeAsync(IReadOnlyCollection<ExtEmployeeDto> employees,
        CancellationToken cancellationToken = default)
        => employees
            .Select(p => new EmployeeDto
            {
                Guid = _guidConverter.Convert(p.employeeGuid),
                PersonGuid = _guidConverter.Convert(p.personGuid),
                DepartmentGuid = _guidConverter.Convert(p.departmentGuid),
                PositionGuid =  _guidConverter.Convert(p.positionGuid),
                IsMain =  p.isMain,
                IsActual =   p.isActual,
                EmploymentDate = p.employmentDate,
                DismissalDate = p.dismissalDate,
                NextPlannedVacationDate =  p.nextPlannedVacationDate,
                VacationRemainingDays = p.remainingDayCount
            })
            .ToList();
}