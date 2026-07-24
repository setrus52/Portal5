namespace IDM.Infrastructure.Options.Idm;

/// <summary>
/// Конфигурация для нормализации имен отделов
/// </summary>
public class DepartmentNormalizationOptions
{
    public IReadOnlyDictionary<string, string> NameReplacements { get; set; } = 
        new Dictionary<string, string>();
}