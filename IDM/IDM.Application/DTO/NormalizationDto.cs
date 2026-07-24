using Common.Enums;

namespace IDM.Application.DTO;

public class NormalizationDto
{
    public int Id { get; set; }
    public NormalizationCategory Category { get; set; }
    public string Pattern { get; set; } = string.Empty;
    public string? Replacement { get; set; } = string.Empty;
    public int? Priority { get; set; }
}