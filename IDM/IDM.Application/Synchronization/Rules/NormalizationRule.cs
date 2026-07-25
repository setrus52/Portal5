namespace IDM.Application.Synchronization.Rules;

public class NormalizationRule
{
    public string Pattern { get; init; } = string.Empty;
    public string Replacement { get; init; } = string.Empty;
    public int Priority { get; init; }
}