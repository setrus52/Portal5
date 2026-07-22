using Common.Enums;

namespace IDM.Domain.Entities;

public class Normalization
{
    public int Id { get; set; }
    public NormalizationType Category { get; set; }
    public string Pattern { get; set; } = string.Empty;
    public string? Replacement { get; set; } = string.Empty;
    public int? Priority { get; set; }
}