using Common.Enums;
using IDM.Application.DTO;
using IDM.Application.Interfaces;
using IDM.Domain.Entities;
using IDM.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IDM.Infrastructure.Services;

public class DepartmentTreeService(IDepartmentsRepository repository) : IDepartmentTreeService
{
    private readonly IDepartmentsRepository _repository = repository;

    public async Task<List<DepartmentTreeItemDto>> GetRootDepartmentsTreeAsync(
        CancellationToken cancellationToken = default)
    {
        var departments = await _repository.Query()
            .Where(p => p.IsActual)
            .ToListAsync(cancellationToken);
        return new List<DepartmentTreeItemDto>();
    }

    private List<DepartmentTreeItemDto> GetRootDepartmentsRecursive(List<Department> departments, Guid guid,
        int level = 0)
    {
        var dList = new List<DepartmentTreeItemDto>();
        var department = departments.FirstOrDefault(p => p.Guid == guid);
        if (department is not null)
        {
            var dDto = new DepartmentDto
            {
                Guid = guid,
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
            dList.Add(new DepartmentTreeItemDto
            {
                Department = dDto,
                Sortorder = level,
                Location = level == 0 ? DepartmentLocation.Current : DepartmentLocation.Superior
            });
            if (department is { ParentGuid: { } parentGuid })
            {
                dList.AddRange(GetRootDepartmentsRecursive(departments, department.ParentGuid, level + 1));
            }
        }

        return dList;
    }
}