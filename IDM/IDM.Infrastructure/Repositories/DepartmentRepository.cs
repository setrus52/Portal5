using Microsoft.EntityFrameworkCore;
using Common.Repositories;
using IDM.Application.Repositories;
using IDM.Domain.Entities;

namespace IDM.Infrastructure.Repositories;

public sealed class DepartmentRepository(AppDbContext context)
    : Repository<Department>(context), IDepartmentsRepository
{
    public Task<Department?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        => Query().FirstOrDefaultAsync(d => d.Guid == guid, cancellationToken);
}