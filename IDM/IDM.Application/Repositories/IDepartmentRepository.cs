using Common.Repositories;
using IDM.Domain.Entities;

namespace IDM.Application.Repositories;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<Department?> GetByGuidAsync(
        Guid guid,
        CancellationToken cancellationToken = default);
}