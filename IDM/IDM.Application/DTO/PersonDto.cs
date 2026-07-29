using Common.Enums;

namespace IDM.Application.DTO;

public class PersonDto
{
    public Guid Guid { get; set; }
    public string? Code { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public string? DisplayName { get; set; }
    public Gender Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Login { get; set; }
    public string? Domain { get; set; }
    public string? Email { get; set; }
    public string? OfficeNumber { get; set; }
    public string? InternalPhone { get; set; }
    public string? MobilePhone { get; set; }
    public string? Photo { get; set; }
}