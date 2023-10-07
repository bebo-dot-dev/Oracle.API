using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Oracle.Database;
using Testcontainers.Oracle;

// ReSharper disable once CheckNamespace
#pragma warning disable CA1050

/// <summary>
/// The non-namespaced Nunit SetupFixture for all test fixtures in this project
/// </summary>
[SetUpFixture]
internal class SetupFixture
{
    private static OracleContainer _oracleTestContainer;
    private static IConfiguration _configuration;

    public static OracleDbContext DbContext;

    [OneTimeSetUp]
    public static async Task OneTimeSetup()
    {
        _oracleTestContainer = new OracleBuilder()
            .WithImage("gvenzl/oracle-xe:slim-faststart")
            .Build();

        await _oracleTestContainer.StartAsync();

        CreateDatabaseContext(_oracleTestContainer.GetConnectionString());
    }

    private static void CreateDatabaseContext(string connectionString)
    {
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new List<KeyValuePair<string, string>>
                {
                    new("ConnectionStrings:oracle", connectionString)
                })
            .Build();

        var options = new DbContextOptions<OracleDbContext>();
        DbContext = new OracleDbContext(options, _configuration);
        DbContext.Database.Migrate();
    }

    public static async Task ClearDownDatabase()
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE ORACLE.\"department\" CASCADE");
    }

    [OneTimeTearDown]
    public static async Task OneTimeTearDown()
    {
        await DbContext.Database.CloseConnectionAsync();
        await DbContext.DisposeAsync();

        await _oracleTestContainer.StopAsync();
        await _oracleTestContainer.DisposeAsync();
    }
}

#pragma warning restore CA1050