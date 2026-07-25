using IDM.Application.DTO;
using IDM.Application.Synchronization;
using IDM.Application.Synchronization.Departments;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Records;

namespace IDM.Infrastructure.Services;

public class IdmDepartmentNormalizer(
    INormalizationProvider normalizationProvider,
    IDepartmentNameNormalizer departmentNameNormalizer,
    IIdmGuidConverter guidConverter)
    : IIdmDepartmentNormalizer
{
    private readonly INormalizationProvider _normalizationProvider = normalizationProvider;
    private readonly IDepartmentNameNormalizer _departmentNameNormalizer = departmentNameNormalizer;
    private readonly IIdmGuidConverter _guidConverter = guidConverter;

    public async Task<List<DepartmentDto>> NormalizeAsync(
        IReadOnlyCollection<ExtDepartmentDto> departments,
        CancellationToken cancellationToken = default)
    {
        var rules = await _normalizationProvider
            .GetDepartmentOptionsAsync(cancellationToken);

        return departments
            .Select(d => new DepartmentDto
            {
                Guid = _guidConverter.Convert(d.guid),
                Name = _departmentNameNormalizer.Normalize(d.name, rules),
                SourceName = d.name,
                ShortName = d.shortName,
                ParentGuid = _guidConverter.ConvertNullable(d.parent),
                SupervisorGuid = _guidConverter.ConvertNullable(d.supervisorGuid),
                IsActual = d.isActual
            })
            .ToList();
    }
}