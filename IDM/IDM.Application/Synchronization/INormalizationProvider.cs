using IDM.Application.Synchronization.Rules;

namespace IDM.Application.Synchronization;

public interface INormalizationProvider
{
    Task<NormalizationRules> GetDepartmentOptionsAsync(
        CancellationToken cancellationToken = default);

    Task<NormalizationRules> GetPhoneBranchOptionsAsync(
        CancellationToken cancellationToken = default);

    Task<NormalizationRules> GetPositionOptionsAsync(
        CancellationToken cancellationToken = default);
}