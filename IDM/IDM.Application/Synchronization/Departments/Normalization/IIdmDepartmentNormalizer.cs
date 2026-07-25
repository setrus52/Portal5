using IDM.Application.DTO;
using IDM.Application.Synchronization.Records;

namespace IDM.Application.Synchronization.Departments.Normalization;

public interface IIdmDepartmentNormalizer
{
    Task<List<DepartmentDto>> NormalizeAsync(
        IReadOnlyCollection<ExtDepartmentDto> departments,
        CancellationToken cancellationToken = default);
}