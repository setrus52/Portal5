using Common.Extensions;
using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDM.Infrastructure.Configurations;

public class EmployeeStatusConfig : IEntityTypeConfiguration<EmployeeStatus>
{
    public void Configure(EntityTypeBuilder<EmployeeStatus> builder)
    {
        builder.ToTable("EmployeeStatuses");
        builder.HasKey(pk => pk.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(p => p.Description)
            .HasColumnType("nvarchar(max)");
        builder.Property(p => p.Icon)
            .HasMaxLength(5000);
        builder.Property(p => p.CssClass)
            .HasMaxLength(5000);
        builder.Property(p => p.EmployeeStatusRules)
            .IsRequired()
            .HasJsonConversion();
    }
}