using Common.Enums;
using IDM.Application.Repositories;
using IDM.Application.Synchronization;
using IDM.Application.Synchronization.Rules;
using Microsoft.EntityFrameworkCore;

namespace IDM.Infrastructure.Synchronization;

public class NormalizationProvider(INormalizationRepository repository)
    : INormalizationProvider
{
    public Task<NormalizationRules> GetDepartmentOptionsAsync(
        CancellationToken cancellationToken = default)
        => GetRulesAsync(NormalizationCategory.Department, cancellationToken);

    public Task<NormalizationRules> GetPhoneBranchOptionsAsync(
        CancellationToken cancellationToken = default)
        => GetRulesAsync(NormalizationCategory.PhoneBranch, cancellationToken);

    public Task<NormalizationRules> GetPositionOptionsAsync(
        CancellationToken cancellationToken = default)
        => GetRulesAsync(NormalizationCategory.Position, cancellationToken);

    private async Task<NormalizationRules> GetRulesAsync(
        NormalizationCategory category,
        CancellationToken cancellationToken)
    {
        var rules = await repository.Query()
            .AsNoTracking()
            .Where(x => x.Category == category)
            .OrderBy(x => x.Priority ?? int.MaxValue)
            .Select(x => new NormalizationRule
            {
                Pattern = x.Pattern,
                Replacement = x.Replacement ?? string.Empty,
                Priority = x.Priority ?? int.MaxValue
            })
            .ToListAsync(cancellationToken);

        return new NormalizationRules
        {
            Rules = rules
        };
    }
}