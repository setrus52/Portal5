using Common.Repositories;
using IDM.Application.Repositories;
using IDM.Domain.Entities;

namespace IDM.Infrastructure.Repositories;

public class PersonRepository(AppDbContext context)
    : Repository<Person>(context), IPersonRepository
{
}