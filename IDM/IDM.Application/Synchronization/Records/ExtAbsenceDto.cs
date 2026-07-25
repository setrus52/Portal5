namespace IDM.Application.Synchronization.Records;

public record ExtAbsenceDto(
    string guid,
    string employeeGuid,
    DateTime startDate,
    DateTime endDate,
    string reason,
    string description
);