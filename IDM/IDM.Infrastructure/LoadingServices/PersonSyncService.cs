using Common.Enums;
using IDM.Application.Abstractions.Synchronization;
using IDM.Application.DTO;
using IDM.Application.Repositories;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Records;
using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IDM.Infrastructure.LoadingServices;

public class PersonSyncService(
    ILogger<PersonSyncService> logger,
    IIdmLoadingService idmLoadingService,
    IIdmNormalizer<PersonDto, ExtPersonDto> personNormalizer,
    IPersonRepository personRepository)
    : ISyncService<Person>
{
    private readonly ILogger<PersonSyncService> _logger = logger;
    private readonly IIdmLoadingService _idmLoadingService = idmLoadingService;
    private readonly IIdmNormalizer<PersonDto, ExtPersonDto> _personNormalizer = personNormalizer;
    private readonly IPersonRepository _personRepository = personRepository;

    public async Task<List<Person>> SyncAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Синхронизация физических лиц начата");

        try
        {
            // 1. Получаем физлица из IDM
            var extPersons = await _idmLoadingService.LoadPersonsAsync(cancellationToken);

            // 2. Нормализуем
            var persons = await _personNormalizer.NormalizeAsync(extPersons, cancellationToken);

            // 3. Получаем существующих физических лиц
            var dbPersonsByGuid = await _personRepository
                .QueryTracking()
                .Include(x => x.Contacts)
                .ToDictionaryAsync(x => x.Guid, cancellationToken);

            // 4. Добавляем новые и обновляем существующие
            var added = 0;
            var updated = 0;
            var changedPersons = new List<Person>(persons.Count);

            foreach (var dto in persons)
            {
                if (dbPersonsByGuid.TryGetValue(dto.Guid, out var person))
                {
                    UpdatePersonInfo(person, dto);
                    UpdateContacts(person, dto);

                    changedPersons.Add(person);
                    updated++;
                }
                else
                {
                    var personEntity = CreatePerson(dto);
                    _personRepository.Add(personEntity);

                    changedPersons.Add(personEntity);
                    added++;
                }
            }
            _logger.LogInformation("Физ. лица. Добавлено: {Added}, обновлено: {Updated}", added, updated);
            return changedPersons;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при синхронизации физических лиц");
            throw;
        }
    }

    private static Person CreatePerson(PersonDto dto)
    {
        var person = new Person
        {
            Guid = dto.Guid,
            Code = dto.Code,
            Surname = dto.Surname,
            Name = dto.Name,
            Patronymic = dto.Patronymic,
            DisplayName = dto.DisplayName,
            Gender = dto.Gender,
            Birthday = dto.Birthday,
            Login = dto.Login,
            Domain = dto.Domain,
            Photo = dto.Photo
        };

        person.Contacts.Add(CreateEmailContact(dto));

        AddPhoneIfExists(person, PersonContactType.OfficePhone, dto.OfficeNumber);
        AddPhoneIfExists(person, PersonContactType.InternalPhone, dto.InternalPhone);
        AddPhoneIfExists(person, PersonContactType.MobilePhone, dto.MobilePhone);

        return person;
    }

    private static void UpdatePersonInfo(Person person, PersonDto dto)
    {
        person.Surname = dto.Surname;
        person.Name = dto.Name;
        person.Patronymic = dto.Patronymic;
        person.DisplayName = dto.DisplayName;
        person.Gender = dto.Gender;
        person.Birthday = dto.Birthday;
        person.Login = dto.Login;
        person.Domain = dto.Domain;
        person.Photo = dto.Photo; // Закомментировать при переходе на локальное хранение фотографий
    }

    private static void UpdateContacts(Person person, PersonDto dto)
    {
        var contacts = person.Contacts
            .Where(c => !c.IsAddedManual)
            .ToDictionary(c => c.Type);

        SyncEmail(dto, person, contacts);

        SyncPhone(dto.OfficeNumber, PersonContactType.OfficePhone, person, contacts);
        SyncPhone(dto.InternalPhone, PersonContactType.InternalPhone, person, contacts);
        SyncPhone(dto.MobilePhone, PersonContactType.MobilePhone, person, contacts);
    }

    private static void SyncEmail(
        PersonDto dto,
        Person person,
        Dictionary<PersonContactType, Contact> contacts)
    {
        var email = GetEmail(dto);

        if (contacts.TryGetValue(PersonContactType.Email, out var contact))
        {
            contact.Value = email;
            contact.IsPrimary = true;
            return;
        }

        person.Contacts.Add(CreateContact(
            PersonContactType.Email,
            email,
            true));
    }

    private static void SyncPhone(
        string? phone,
        PersonContactType type,
        Person person,
        Dictionary<PersonContactType, Contact> contacts)
    {
        if (contacts.TryGetValue(type, out var contact))
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                person.Contacts.Remove(contact);
                return;
            }

            contact.Value = phone;
            return;
        }

        if (!string.IsNullOrWhiteSpace(phone)) person.Contacts.Add(CreateContact(type, phone));
    }

    private static void AddPhoneIfExists(
        Person person,
        PersonContactType type,
        string? phone)
    {
        if (!string.IsNullOrWhiteSpace(phone)) person.Contacts.Add(CreateContact(type, phone));
    }

    private static Contact CreateEmailContact(PersonDto dto) =>
        CreateContact(
            PersonContactType.Email,
            GetEmail(dto),
            true);

    private static string GetEmail(PersonDto dto) =>
        string.IsNullOrWhiteSpace(dto.Email)
            ? $"{dto.Login}@yuresk.ru"
            : dto.Email;

    private static Contact CreateContact(
        PersonContactType type,
        string value,
        bool isPrimary = false) =>
        new()
        {
            Type = type,
            Value = value,
            IsPrimary = isPrimary,
            IsAddedManual = false
        };
}