namespace IDM.Application.DTO;

public class PositionDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Order { get; set; }
}