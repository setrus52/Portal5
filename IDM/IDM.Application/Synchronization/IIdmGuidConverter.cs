namespace IDM.Application.Synchronization;

public interface IIdmGuidConverter
{
    Guid Convert(string value);
    Guid? ConvertNullable(string? value);
}