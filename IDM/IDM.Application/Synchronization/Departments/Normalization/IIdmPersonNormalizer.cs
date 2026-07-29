using IDM.Application.DTO;
using IDM.Application.Synchronization.Records;

namespace IDM.Application.Synchronization.Departments.Normalization;

public interface IIdmPersonNormalizer
{
    Task<List<PersonDto>> NormalizeAsync(
        IReadOnlyCollection<ExtPersonDto> departments,
        CancellationToken cancellationToken = default);
}