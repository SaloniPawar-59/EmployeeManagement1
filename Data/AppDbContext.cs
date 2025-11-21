using EmployeeManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Employee> Employees => Set<Employee>();
}

