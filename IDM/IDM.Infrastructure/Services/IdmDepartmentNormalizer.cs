using IDM.Application.DTO;
using IDM.Application.Synchronization;
using IDM.Application.Synchronization.Departments;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Records;

namespace IDM.Infrastructure.Services;

public class IdmDepartmentNormalizer(
    INormalizationProvider normalizationProvider,
    IFieldNormalizer fieldNormalizer,
    IIdmGuidConverter guidConverter)
    : IIdmNormalizer<DepartmentDto, ExtDepartmentDto>
{
    private readonly INormalizationProvider _normalizationProvider = normalizationProvider;
    private readonly IFieldNormalizer _fieldNormalizer = fieldNormalizer;
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
                Name = _fieldNormalizer.Normalize(d.name, rules),
                SourceName = d.name,
                ShortName = d.shortName,
                ParentGuid = _guidConverter.ConvertNullable(d.parent),
                SupervisorGuid = _guidConverter.ConvertNullable(d.supervisorGuid),
                IsActual = d.isActual
            })
            .ToList();
    }
}