namespace IDM.Application.DTO;

public class DepartmentDto
{
    public Guid Guid { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsActual { get; set; }

    public string? ShortName { get; set; }

    public string SourceName { get; set; } = string.Empty;

    public bool IsManual { get; set; }

    public int? OrderIndex { get; set; }

    public Guid? ParentGuid { get; set; }

    public bool IsParentDepartmentDefinedManually { get; set; }

    public bool IsHeadOfBranch { get; set; }

    public bool IsShowOnScheme { get; set; }

    public Guid? SupervisorGuid { get; set; }
    public bool? IsSupervisorDefinedManual { get; set; }
}