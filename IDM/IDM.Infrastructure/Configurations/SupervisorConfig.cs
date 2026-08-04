using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDM.Infrastructure.Configurations;

public class SupervisorConfig : IEntityTypeConfiguration<Supervisor>
{
    public void Configure(EntityTypeBuilder<Supervisor> builder)
    {
        builder.ToTable("Supervisors");

        builder.HasKey(x => x.DepartmentGuid);

        builder.Property(x => x.DepartmentGuid)
            .ValueGeneratedNever();

        builder.Property(x => x.EmployeeGuid)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(x => x.IsManual)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.HasOne(x => x.Department)
            .WithOne(x => x.Supervisor)
            .HasForeignKey<Supervisor>(x => x.DepartmentGuid)
            .OnDelete(DeleteBehavior.Cascade);
    }
}