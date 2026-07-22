using Common.Enums;

namespace IDM.Application.DTO;

public class DepartmentTreeItemDto
{
    public DepartmentDto Department { get; set; } = null!;
    public DepartmentLocation Location { get; set; }
    public int Sortorder { get; set; }
}