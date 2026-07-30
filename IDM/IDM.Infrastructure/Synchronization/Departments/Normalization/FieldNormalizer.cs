using System.Text.RegularExpressions;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Rules;

namespace IDM.Infrastructure.Synchronization.Departments.Normalization;

public class FieldNormalizer : IFieldNormalizer
{
    public string Normalize(string sourceName, NormalizationRules rules)
    {
        var result = sourceName;

        foreach (var rule in rules.Rules)
        {
            result = result.Replace(
                rule.Pattern,
                rule.Replacement,
                StringComparison.OrdinalIgnoreCase);
        }

        return Regex.Replace(result.Trim(), @"\s+", " ");
    }
}