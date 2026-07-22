using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDM.Infrastructure.Configurations;

public class AbsenceConfig : IEntityTypeConfiguration<Absence>
{
    public void Configure(EntityTypeBuilder<Absence> builder)
    {
        builder.ToTable("Absences");
        builder.HasKey(x => x.Id);

        builder.Property(p => p.ExternalGuid).HasMaxLength(50);
        builder.Property(p => p.Reason).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(250);

        builder.Property(p => p.Start).HasColumnType("datetime2");
        builder.Property(p => p.End).HasColumnType("datetime2");

        builder.HasOne(p => p.Employee)
            .WithMany(s => s.Absences)
            .HasForeignKey(p => p.EmployeeGuid)
            .OnDelete(DeleteBehavior.Cascade);
    }
}