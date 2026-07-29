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
        builder.HasIndex(p => p.Code)
            .IsUnique();
        builder.HasIndex(p => p.Login)
            .IsUnique()
            .HasFilter("[Login] IS NOT NULL");

        builder.Property(p => p.Guid)
            .IsRequired()
            .ValueGeneratedNever();
        builder.Property(p => p.Code)
            .IsRequired()
            .ValueGeneratedNever()
            .HasMaxLength(32);
        builder.Property(p => p.Surname)
            .HasMaxLength(100);
        builder.Property(p => p.Name)
            .HasMaxLength(100);
        builder.Property(p => p.Patronymic)
            .HasMaxLength(100);
        builder.Property(p => p.Birthday)
            .HasColumnType("date");
        builder.Property(p => p.Login)
            .HasMaxLength(100);
    }
}