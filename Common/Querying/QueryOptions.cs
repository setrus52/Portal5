namespace Common.Querying;

/// <summary>
/// Универсальные параметры запроса.
/// </summary>
public sealed class QueryOptions
{
    /// <summary>
    /// Поля для выборки.
    /// Пример:
    /// Id,Name,Department.Name
    /// </summary>
    public string? Select { get; init; }

    /// <summary>
    /// Фильтр.
    /// Примеры:
    /// Name:Иван
    /// Age>18
    /// Department.Name:ИТ
    /// </summary>
    public string? Filter { get; init; }

    /// <summary>
    /// Сортировка.
    /// Пример:
    /// Name,-Department.Name
    /// </summary>
    public string? OrderBy { get; init; }

    /// <summary>
    /// Навигационные свойства.
    /// Пример:
    /// Department,Manager
    /// </summary>
    public string? Include { get; init; }

    public int Skip { get; init; }

    public int Take { get; init; } = 100;

    /// <summary>
    /// Отключить отслеживание сущностей.
    /// </summary>
    public bool AsNoTracking { get; init; } = true;
}