using Common.Enums;

namespace IDM.Domain.Entities;

public class Contact
{
    public int Id { get; set; }

    /// <summary>
    /// Тип контактной информации
    /// </summary>
    public PersonContactType Type { get; set; }

    /// <summary>
    /// Значение контакта
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Основной контакт данного типа
    /// </summary>
    public bool IsPrimary { get; set; }

    public Guid PersonGuid { get; set; }
    public Person Person { get; set; } = null!;
}