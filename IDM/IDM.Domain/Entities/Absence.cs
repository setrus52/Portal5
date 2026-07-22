namespace IDM.Domain.Entities;

public class Absence
{
    public int Id { get; set; }

    public string ExternalGuid { get; set; } = string.Empty;

    public Guid EmployeeGuid { get; set; }
    public Employee Employee { get; set; } = null!;

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}