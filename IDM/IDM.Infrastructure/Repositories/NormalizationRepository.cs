using Common.Enums;
using Microsoft.EntityFrameworkCore;
using Common.Repositories;
using IDM.Application.DTO;
using IDM.Application.Repositories;
using IDM.Domain.Entities;

namespace IDM.Infrastructure.Repositories;

public class NormalizationRepository(AppDbContext context)
    : Repository<Normalization>(context), INormalizationRepository
{
    public async Task<List<Normalization>> GetByCategoryAsync(NormalizationCategory category)
        => await Query().Where(p => p.Category == category)
            .ToListAsync();
}