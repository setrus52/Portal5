using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDM.Infrastructure.Configurations;

public class SupervisorConfig : IEntityTypeConfiguration<Supervisor>
{
    public void Configure(EntityTypeBuilder<Supervisor> builder)
    {
        builder.ToTable("Supervisors");
        builder.HasKey(p => new { p.DepartmentGuid, p.EmployeeGuid });

        builder.Property(p => p.DepartmentGuid)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(p => p.EmployeeGuid)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(p => p.IsManual)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);
    }
}