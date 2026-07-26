using IDM.Application.Synchronization.Rules;

namespace IDM.Application.Synchronization.Departments.Normalization;

public interface IPositionNameNormalizer
{
    string Normalize(string sourceName, NormalizationRules rules);
}