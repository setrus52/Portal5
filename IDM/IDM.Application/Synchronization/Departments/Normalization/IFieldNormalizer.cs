using IDM.Application.Synchronization.Rules;

namespace IDM.Application.Synchronization.Departments.Normalization;

public interface IFieldNormalizer
{
    string Normalize(string value, NormalizationRules rules);
}