namespace IDM.Application.Synchronization.Records;

public record ExtEmployeeDto(
    string employeeGuid,
    string personGuid,
    string departmentGuid,
    string positionGuid,
    bool isMain,
    bool isActual,
    DateTime employmentDate,
    DateTime? dismissalDate,
    DateTime? nextPlannedVacationDate,
    decimal? remainingDayCount);