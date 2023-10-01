using Oracle.Database;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.RegisterOracleDatabaseServices(builder.Configuration);

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
}

app.Run();