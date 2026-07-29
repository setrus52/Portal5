using System.ComponentModel.DataAnnotations.Schema;
using Common.Enums;

namespace IDM.Domain.Entities;

public class Person
{
    /// <summary>
    /// GUID физлица
    /// </summary>
    public Guid Guid { get; set; }

    /// <summary>
    /// Код физлица из 1С
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Отчество
    /// </summary>
    public string? Patronymic { get; set; }

    public string? DisplayName { get; set; }

    /// <summary>
    /// Пол
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// Логин в домене
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Домен
    /// </summary>
    public string? Domain { get; set; }

    public string? Photo { get; set; }

    /// <summary>
    /// Список должностей, на которых работает сотрудник
    /// </summary>
    public ICollection<Employee> EmployeePositions { get; set; } = new List<Employee>();

    /// <summary>
    /// Контакты
    /// </summary>
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}