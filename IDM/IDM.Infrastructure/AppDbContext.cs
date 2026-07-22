using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDM.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    #region Организация

    public DbSet<Absence> Absences { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeStatus> EmployeeStatuses { get; set; }
    public DbSet<Normalization> Normalizations { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Supervisor> Supervisors { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Находим и применяем все классы, реализующие IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}