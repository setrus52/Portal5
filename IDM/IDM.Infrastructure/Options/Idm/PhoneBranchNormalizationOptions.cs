namespace IDM.Infrastructure.Options.Idm;

public class PhoneBranchNormalizationOptions
{
    public IReadOnlyDictionary<string, string> Branches { get; set; } = 
        new Dictionary<string, string>();
}