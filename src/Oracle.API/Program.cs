using System.Reflection;
using Oracle.Database;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(o =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.RegisterOracleDatabaseServices(builder.Configuration);
builder.Services.AddMediator(o => o.ServiceLifetime = ServiceLifetime.Scoped);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
try
{
    app.Services.MigrateDatabase(builder.Configuration);
    logger.LogInformation("EF Core Oracle database migrations ran successfully");
}
catch (Exception e)
{
    logger.LogError(e, "EF Core Oracle database migrations failure");
    Environment.ExitCode = -1;
    return;
}

app.Run();