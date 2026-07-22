using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Common.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<List<T>> HasJsonConversion<T>(
        this PropertyBuilder<List<T>> propertyBuilder,
        JsonSerializerOptions? jsonOptions = null)
    {
        ArgumentNullException.ThrowIfNull(propertyBuilder);

        var converter = new ValueConverter<List<T>, string>(
            value => JsonSerializer.Serialize(value, jsonOptions),
            value => JsonSerializer.Deserialize<List<T>>(value, jsonOptions) ?? new List<T>());

        var comparer = new ValueComparer<List<T>>(
            (left, right) => left!.SequenceEqual(right!),
            list => list.Aggregate(
                0,
                (hash, item) => HashCode.Combine(hash, item == null ? 0 : item.GetHashCode())),
            list => list.ToList());

        propertyBuilder
            .HasConversion(converter)
            .Metadata.SetValueComparer(comparer);

        return propertyBuilder;
    }
}