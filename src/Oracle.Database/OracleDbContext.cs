using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.Core.Outgoing;

namespace Oracle.Database;

internal sealed class OracleDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public OracleDbContext(DbContextOptions<OracleDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    
    public DbSet<DepartmentData> Departments { get; set; } = null!;
    public DbSet<EmployeeData> Employees { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseOracle(_configuration.GetConnectionString("oracle"), o =>
            {
                o.CommandTimeout(5);
            });
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OracleDbContext).Assembly);
    }
}