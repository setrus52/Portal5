using IDM.Application.DTO;
using IDM.Application.Synchronization;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Records;

namespace IDM.Infrastructure.Services;

public class IdmPositionNormalizer(
    INormalizationProvider normalizationProvider,
    IFieldNormalizer fieldNormalizer,
    IIdmGuidConverter guidConverter
) : IIdmNormalizer<PositionDto, ExtPositionDto>
{
    private readonly INormalizationProvider _normalizationProvider = normalizationProvider;
    private readonly IFieldNormalizer _fieldNormalizer = fieldNormalizer;
    private readonly IIdmGuidConverter _guidConverter = guidConverter;

    public async Task<List<PositionDto>> NormalizeAsync(
        IReadOnlyCollection<ExtPositionDto> positions,
        CancellationToken cancellationToken = default)
    {
        var rules = await _normalizationProvider
            .GetPositionOptionsAsync(cancellationToken);

        return positions
            .Select(p => new PositionDto
            {
                Guid = _guidConverter.Convert(p.guid),
                Name = _fieldNormalizer.Normalize(p.name, rules),
                Order = p.orderPos
            })
            .ToList();
    }
}