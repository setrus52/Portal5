using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Rules;

namespace IDM.Infrastructure.Synchronization.Departments.Normalization;

public class DepartmentNameNormalizer : IDepartmentNameNormalizer
{
    public string Normalize(
        string sourceName,
        NormalizationRules rules)
    {
        var result = sourceName;

        foreach (var rule in rules.Rules)
            result = result.Replace(
                rule.Pattern,
                rule.Replacement,
                StringComparison.OrdinalIgnoreCase);

        return result.Trim();
    }
}