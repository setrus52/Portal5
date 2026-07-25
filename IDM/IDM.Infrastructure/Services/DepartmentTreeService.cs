using Common.Enums;
using IDM.Application.DTO;
using IDM.Application.Interfaces;
using IDM.Application.Repositories;
using IDM.Domain.Entities;
using IDM.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IDM.Infrastructure.Services;

public class DepartmentTreeService(IDepartmentRepository repository) : IDepartmentTreeService
{
    private readonly IDepartmentRepository _repository = repository;

    public async Task<List<DepartmentTreeItemDto>> GetRootDepartmentsTreeAsync(
        CancellationToken cancellationToken = default)
    {
        var departments = await _repository.Query()
            .Where(p => p.IsActual)
            .ToListAsync(cancellationToken);
        return new List<DepartmentTreeItemDto>();
    }

    private List<DepartmentTreeItemDto> GetDepartmentAncestorsRecursive(
        List<Department> departments,
        Guid guid,
        int level = 0)
    {
        var result = new List<DepartmentTreeItemDto>();

        var department = departments.FirstOrDefault(p => p.Guid == guid);

        if (department is null)
            return result;


        var dto = new DepartmentDto
        {
            Guid = department.Guid,
            Name = department.Name,
            IsActual = department.IsActual,
            ShortName = department.ShortName,
            SourceName = department.SourceName,
            IsManual = department.IsManual,
            OrderIndex = department.OrderIndex,
            ParentGuid = department.ParentGuid,
            IsParentDepartmentDefinedManually = department.IsParentDepartmentDefinedManually,
            IsHeadOfBranch = department.IsHeadOfBranch,
            IsShowOnScheme = department.IsShowOnScheme,
            SupervisorGuid = department.Supervisor?.EmployeeGuid,
            IsSupervisorDefinedManual = department.Supervisor?.IsManual
        };


        result.Add(new DepartmentTreeItemDto
        {
            Department = dto,
            Sortorder = level,
            Location = level == 0
                ? DepartmentLocation.Current
                : DepartmentLocation.Superior
        });


        if (department.ParentGuid.HasValue)
            result.AddRange(
                GetDepartmentAncestorsRecursive(
                    departments,
                    department.ParentGuid.Value,
                    level + 1));


        return result;
    }
}