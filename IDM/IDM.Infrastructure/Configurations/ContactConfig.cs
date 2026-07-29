using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDM.Infrastructure.Configurations;

public class ContactConfig : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");
        builder.HasKey(p => p.Id);
        builder.HasIndex(x => new
        {
            x.PersonGuid,
            x.Type,
            x.IsPrimary
        });

        builder.Property(p => p.Value)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(p => p.IsPrimary)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.HasOne(p => p.Person)
            .WithMany(p => p.Contacts)
            .HasForeignKey(p => p.PersonGuid)
            .OnDelete(DeleteBehavior.Cascade);
    }
}