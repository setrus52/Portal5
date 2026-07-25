namespace IDM.Domain.Entities;

public class Department
{
    /// <summary>
    /// GUID отдела
    /// </summary>
    public Guid Guid { get; set; }

    /// <summary>
    /// Наименование отдела
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Актуальность отдела
    /// </summary>
    public bool IsActual { get; set; }

    /// <summary>
    /// Короткое наименование
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    /// Исходное название отдела в учетной системе
    /// </summary>
    public string SourceName { get; set; } = string.Empty;

    /// <summary>
    /// Флаг. Отдел создан вручную
    /// </summary>
    public bool IsManual { get; set; }

    /// <summary>
    /// Параметр сортировки
    /// </summary>
    public int? OrderIndex { get; set; }

    #region Опроеделение древовидной структуры отделов

    /// <summary>
    /// GUID Родительского отдела
    /// </summary>
    public Guid? ParentGuid { get; set; }

    public Department? Parent { get; set; }

    /// <summary>
    /// Флаг. Устанавливается в true, если родительское подразделение определено вручную
    /// </summary>
    public bool IsParentDepartmentDefinedManually { get; set; }

    /// <summary>
    /// Список ссылок на подчиненные отделы
    /// </summary>
    public ICollection<Department> Subordinates { get; set; } = new List<Department>();

    public void SetParent(Guid? parentGuid)
    {
        if (Guid == parentGuid)
            throw new InvalidOperationException(
                "Подразделение не может быть родительским элементом для самого себя.");

        ParentGuid = parentGuid;
        IsParentDepartmentDefinedManually = true;
    }

    #endregion


    /// <summary>
    /// Флаг филиала
    /// </summary>
    public bool IsHeadOfBranch { get; set; }

    public bool IsShowOnScheme { get; set; }

    #region Определение руководителя

    public Supervisor? Supervisor { get; set; }

    public void SetSupervisor(Guid employeeGuid)
    {
        if (Supervisor is null)
        {
            Supervisor = new Supervisor
            {
                EmployeeGuid = employeeGuid,
                IsManual = true
            };
        }
        else
        {
            Supervisor.EmployeeGuid = employeeGuid;
            Supervisor.IsManual = true;
        }
    }

    public void RemoveSupervisor()
    {
        if (Supervisor is not null)
            Supervisor = null;
    }

    #endregion


    #region Определение сотрудников отдела

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    #endregion
}