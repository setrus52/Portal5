using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDM.Infrastructure.Configurations;

public class DepartmentConfig : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");
        builder.HasKey(p => p.Guid);

        builder.Property(p => p.Guid)
            .IsRequired().ValueGeneratedNever();
        builder.Property(p => p.SourceName)
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(p => p.ShortName)
            .HasMaxLength(1000);
        builder.Property(p => p.IsManual)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(p => p.ParentGuid)
            .ValueGeneratedNever();
        builder.HasOne(p => p.Parent)
            .WithMany(p => p.Subordinates)
            .HasForeignKey(p => p.ParentGuid)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.IsParentDepartmentDefinedManually)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);
        builder.Property(p => p.IsActual)
            .HasColumnType("bit")
            .HasDefaultValue(true);
        builder.Property(p => p.IsManual)
            .HasColumnType("bit")
            .HasDefaultValue(false);
        builder.Property(p => p.IsHeadOfBranch)
            .HasColumnType("bit")
            .HasDefaultValue(false);
        builder.Property(p => p.IsShowOnScheme)
            .HasColumnType("bit")
            .HasDefaultValue(true);
    }
}