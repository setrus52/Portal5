namespace IDM.Application.DTO;

public class EmployeeDto
{
    public Guid Guid { get; set; }
    public Guid PersonGuid { get; set; }
    public string? EmployeeNumber { get; set; }
    public Guid DepartmentGuid { get; set; }
    public Guid PositionGuid { get; set; }
    public bool IsMain { get; set; }
    public bool IsActual { get; set; }
    public DateTime EmploymentDate { get; set; }
    public DateTime? DismissalDate { get; set; }
    public DateTime? NextPlannedVacationDate { get; set; }
    public decimal? VacationRemainingDays { get; set; }
}