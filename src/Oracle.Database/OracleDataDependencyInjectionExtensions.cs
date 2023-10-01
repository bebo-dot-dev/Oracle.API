using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oracle.Core.Outgoing;

namespace Oracle.Database;

public static class OracleDataDependencyInjectionExtensions
{
    public static IServiceCollection RegisterOracleDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<OracleDbContext>((_, options) =>
        {
            options.UseOracle(configuration.GetConnectionString("oracle"));
        });
        
        services.AddScoped<DbContext, OracleDbContext>();

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        
        return services;
    }

    /// <summary>
    /// Applies database migrations at runtime
    /// Good enough for this demo application, this approach is inappropriate for managing production databases
    /// See https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/></param>
    /// <param name="configuration">The <see cref="IConfiguration"/></param>
    public static void MigrateDatabase(
        this IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        var options = new DbContextOptions<OracleDbContext>();
        using var dbContext = new OracleDbContext(options, configuration);
        dbContext.Database.Migrate();
    }
}