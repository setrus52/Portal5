namespace IDM.Infrastructure.Options.Idm;

public class PositionNormalizationOptions
{
    public IReadOnlyDictionary<string, string> Positions { get; set; } = 
        new Dictionary<string, string>();
}