namespace IDM.Domain.Entities;

public class Supervisor
{
    public Guid DepartmentGuid { get; set; }
    public Department Department { get; set; } = null!;

    public Guid EmployeeGuid { get; set; }
    public Employee Employee { get; set; } = null!;

    public bool IsManual { get; set; }
}