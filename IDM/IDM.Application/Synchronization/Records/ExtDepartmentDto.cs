namespace IDM.Application.Synchronization.Records;

public record ExtDepartmentDto(
    string guid,
    string name,
    string? shortName,
    string? parent,
    string? supervisorGuid,
    string type,
    bool isActual);