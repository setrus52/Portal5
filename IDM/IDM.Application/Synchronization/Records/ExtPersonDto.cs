using Common.Enums;

namespace IDM.Application.Synchronization.Records;

public record ExtPersonDto(
    string guid,
    string code,
    string surname,
    string name,
    string patronymic,
    Gender gender,
    DateTime birthday,
    string login,
    string domain,
    string email,
    string officeNumber,
    string internalPhone,
    string mobilePhone,
    string photo);