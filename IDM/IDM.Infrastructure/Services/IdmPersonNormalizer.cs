using IDM.Application.DTO;
using IDM.Application.Synchronization;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Records;

namespace IDM.Infrastructure.Services;

public class IdmPersonNormalizer(IIdmGuidConverter guidConverter) : IIdmPersonNormalizer
{
    private readonly IIdmGuidConverter _guidConverter = guidConverter;

    public async Task<List<PersonDto>> NormalizeAsync(
        IReadOnlyCollection<ExtPersonDto> persons,
        CancellationToken cancellationToken = default)
    {
        return persons
            .Select(p => new PersonDto
            {
                Guid = _guidConverter.Convert(p.guid),
                Code = p.code,
                Surname = p.surname,
                Name = p.name,
                Patronymic = p.patronymic,
                Gender = p.gender,
                Birthday = p.birthday,
                Login = p.login,
                Email = p.email,
                Domain = p.domain,
                OfficeNumber = p.officeNumber,
                InternalPhone = p.internalPhone,
                MobilePhone = p.mobilePhone,
                Photo = p.photo
            })
            .ToList();
    }
}