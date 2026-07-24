using Common.Repositories;
using IDM.Application.Interfaces;
using IDM.Application.Repositories;
using IDM.Infrastructure.Repositories;

namespace IDM.Infrastructure.Services;

public sealed class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentService(
        IDepartmentRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
}