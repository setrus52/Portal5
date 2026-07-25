namespace IDM.Application.Synchronization.Rules;

public class NormalizationRules
{
    //public Dictionary<string, string?> NameReplacements { get; init; } = new();
    public IReadOnlyList<NormalizationRule> Rules { get; init; }
        = [];
}