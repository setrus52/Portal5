using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDM.Infrastructure.Configurations;

public class PersonConfig : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");
        builder.HasKey(p => p.Guid);

        builder.Property(p => p.Guid)
            .IsRequired()
            .ValueGeneratedNever();
        builder.Property(p => p.Code)
            .HasMaxLength(32);
        builder.Property(p => p.Surname)
            .HasMaxLength(100);
        builder.Property(p => p.Name)
            .HasMaxLength(100);
        builder.Property(p => p.Patronymic)
            .HasMaxLength(100);
        builder.Property(p => p.Birthday)
            .HasColumnType("datetime2");
        builder.Property(p => p.Login)
            .HasMaxLength(100);
    }
}