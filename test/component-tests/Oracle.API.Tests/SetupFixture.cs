using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Oracle.Database;
using Testcontainers.Oracle;

// ReSharper disable once CheckNamespace

/// <summary>
/// The non-namespaced Nunit SetupFixture for all test fixtures in this project
/// </summary>
[SetUpFixture]
internal class SetupFixture
{
    private static WebApplicationFactory<Program> _application;
    private static OracleContainer _oracleTestContainer;

    public static HttpClient Client;
    public static OracleDbContext DbContext;

    [OneTimeSetUp]
    public static async Task OneTimeSetup()
    {
        _oracleTestContainer = new OracleBuilder()
            .WithImage("gvenzl/oracle-xe:slim-faststart")
            .Build();

        await _oracleTestContainer.StartAsync();

        var connectionString = _oracleTestContainer.GetConnectionString();
        _application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(
                b => { b.UseSetting("ConnectionStrings:oracle", connectionString); });

        Client = _application.CreateClient();
        CreateDatabaseContext(connectionString);
    }

    private static void CreateDatabaseContext(string connectionString)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new List<KeyValuePair<string, string>>
                {
                    new("ConnectionStrings:oracle", connectionString)
                }!)
            .Build();

        var options = new DbContextOptions<OracleDbContext>();
        DbContext = new OracleDbContext(options, configuration);
    }

    public static async Task ClearDownDatabase()
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE ORACLE.\"department\" CASCADE");
    }

    [OneTimeTearDown]
    public static async Task OneTimeTearDown()
    {
        await _oracleTestContainer.StopAsync();
        await _oracleTestContainer.DisposeAsync();
        await _application.DisposeAsync();
    }
}