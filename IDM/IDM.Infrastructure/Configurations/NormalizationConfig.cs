using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDM.Infrastructure.Configurations;

public class NormalizationConfig : IEntityTypeConfiguration<Normalization>
{
    public void Configure(EntityTypeBuilder<Normalization> builder)
    {
        builder.ToTable("Normalizations");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Pattern)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Replacement)
            .HasMaxLength(1000);
    }
}