using Common.Repositories;
using IDM.Application.Repositories;
using IDM.Domain.Entities;

namespace IDM.Infrastructure.Repositories;

public class EmployeeRepository(AppDbContext context)
    : Repository<Employee>(context), IEmployeeRepository
{
}