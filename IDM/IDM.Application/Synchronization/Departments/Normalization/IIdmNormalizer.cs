namespace IDM.Application.Synchronization.Departments.Normalization;

public interface IIdmNormalizer<TDestination, TSource>
{
    Task<List<TDestination>> NormalizeAsync(
        IReadOnlyCollection<TSource> source,
        CancellationToken cancellationToken = default);
}