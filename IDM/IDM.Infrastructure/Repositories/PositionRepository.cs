using Common.Repositories;
using IDM.Application.Repositories;
using IDM.Domain.Entities;

namespace IDM.Infrastructure.Repositories;

public class PositionRepository(AppDbContext context) :
    Repository<Position>(context), IPositionRepository
{
}