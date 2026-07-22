namespace IDM.Domain.Entities;

public class EmployeeStatus
{
    public int Id { get; set; }

    /// <summary>
    /// Наименование статуса
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание статуса
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// Иконка статуса 
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// CSS класс для статуса
    /// </summary>
    public string? CssClass { get; set; }

    /// <summary>
    /// Список текстов отсутствий для статуса
    /// </summary>
    public List<string> EmployeeStatusRules { get; set; } = new();
}