using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDM.Infrastructure.Configurations;

public class EmployeeConfig : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(p => p.Guid);

        builder.Property(p => p.Guid)
            .IsRequired()
            .ValueGeneratedNever();
        builder.HasOne(p => p.Person)
            .WithMany(s => s.EmployeePositions)
            .HasForeignKey(p => p.PersonGuid)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Department)
            .WithMany(s => s.Employees)
            .HasForeignKey(p => p.DepartmentGuid)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Position)
            .WithMany(s => s.EmployeesInPosition)
            .HasForeignKey(p => p.PositionGuid)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.IsMain)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(true);

        builder.Property(p => p.EmployeeNumber)
            .HasMaxLength(100);
        
        builder.Property(p => p.IsActual)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(true);

        builder.Property(p => p.EmploymentDate)
            .HasColumnType("datetime2");

        builder.Property(p => p.DismissalDate)
            .HasColumnType("datetime2");

        builder.Property(p => p.NextPlannedVacationDate)
            .HasColumnType("datetime2");

        builder.Property(p => p.VacationRemainingDays)
            .HasColumnType("decimal")
            .HasPrecision(6, 2);

        builder.HasMany(p => p.HeadOfDepartments)
            .WithOne(s => s.Employee)
            .HasForeignKey(p => p.EmployeeGuid)
            .OnDelete(DeleteBehavior.Restrict);
    }
}