using IDM.Application.DTO;
using IDM.Application.Synchronization.Records;

namespace IDM.Application.Synchronization.Departments.Normalization;

public interface IIdmPositionNormalizer
{
    Task<List<PositionDto>> NormalizeAsync(
        IReadOnlyCollection<ExtPositionDto> positions,
        CancellationToken cancellationToken = default);
}