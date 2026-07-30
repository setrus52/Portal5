namespace IDM.Domain.Entities;

public class Employee
{
    /// <summary>
    /// GUID сотрудника
    /// </summary>
    public Guid Guid { get; set; }


    /// <summary>
    /// GUID физлица
    /// </summary>
    public Guid PersonGuid { get; set; }

    /// <summary>
    /// Ссылка на физлицо
    /// </summary>
    public Person Person { get; set; } = null!;

    public string? EmployeeNumber { get; set; }

    /// <summary>
    /// GUID отдела
    /// </summary>
    public Guid DepartmentGuid { get; set; }

    /// <summary>
    /// Ссылка на отдел
    /// </summary>
    public Department Department { get; set; } = null!;

    /// <summary>
    /// GUID должности
    /// </summary>
    public Guid PositionGuid { get; set; }

    /// <summary>
    /// Ссылка на должность
    /// </summary>
    public Position Position { get; set; } = null!;


    /// <summary>
    /// Основное место работы
    /// </summary>
    public bool IsMain { get; set; }

    /// <summary>
    /// Актуальность
    /// </summary>
    public bool IsActual { get; set; }

    /// <summary>
    /// Дата трудоустройства
    /// </summary>
    public DateTime EmploymentDate { get; set; }

    /// <summary>
    /// Дата увольнения
    /// </summary>
    public DateTime? DismissalDate { get; set; }


    public DateTime? NextPlannedVacationDate { get; set; }
    public decimal? VacationRemainingDays { get; set; }

    /// <summary>
    /// Руководитель отделов
    /// </summary>
    public ICollection<Supervisor> HeadOfDepartments { get; set; } = new List<Supervisor>();

    /// <summary>
    /// Отсутствия
    /// </summary>
    public ICollection<Absence> Absences { get; set; } = new List<Absence>();
}