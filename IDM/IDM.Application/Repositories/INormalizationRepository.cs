using Common.Enums;
using Common.Repositories;
using IDM.Application.DTO;
using IDM.Domain.Entities;

namespace IDM.Application.Repositories;

public interface INormalizationRepository : IRepository<Normalization>
{
    Task<List<Normalization>> GetByCategoryAsync(NormalizationCategory category);
}