using IDM.Application.Synchronization;

namespace IDM.Infrastructure.Synchronization;

public class IdmGuidConverter : IIdmGuidConverter
{
    public Guid Convert(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Значение GUID не может быть пустым.", nameof(value));

        /*if (!value.StartsWith("1-") || !value.StartsWith("2-"))
            throw new FormatException($"Некорректный внешний GUID '{value}'. Ожидался префикс '1-' или '2-'.");*/

        return !Guid.TryParse(value.AsSpan(2), out var guid)
            ? throw new FormatException($"Некорректный GUID '{value}'.")
            : guid;
    }

    public Guid? ConvertNullable(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return Convert(value);
    }
}