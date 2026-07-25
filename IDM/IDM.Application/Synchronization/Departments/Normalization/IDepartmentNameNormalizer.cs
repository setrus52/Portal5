using IDM.Application.Synchronization.Rules;

namespace IDM.Application.Synchronization.Departments.Normalization;

public interface IDepartmentNameNormalizer
{
    string Normalize(string sourceName, NormalizationRules rules);
}