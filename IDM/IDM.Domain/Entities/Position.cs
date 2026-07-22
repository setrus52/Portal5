namespace IDM.Domain.Entities;

public class Position
{
    /// <summary>
    /// GUID должности
    /// </summary>
    public Guid Guid { get; set; }

    /// <summary>
    /// Наименовение должности
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Порядок вывода должностей
    /// </summary>
    public int? Order { get; set; }

    public bool IsActual { get; set; }

    /// <summary>
    /// Список сотрудник сотрудников в этой должности
    /// </summary>
    public ICollection<Employee> EmployeesInPosition { get; set; } = new List<Employee>();
}