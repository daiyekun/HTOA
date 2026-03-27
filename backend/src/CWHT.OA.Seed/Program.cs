using CWHT.OA.Seed.Generators;
using Microsoft.Extensions.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting database seeding...");

    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    var connectionString = configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Connection string 'Default' not found.");

    var fsql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.PostgreSQL, connectionString)
        .UseAutoSyncStructure(false)
        .UseMonitorCommand(cmd => Log.Information($"SQL: {cmd.CommandText}"))
        .Build();

    Log.Information("Seeding system data...");
    await SystemDataGenerator.SeedAsync(fsql);

    Log.Information("Seeding workflow data...");
    await WorkflowDataGenerator.SeedAsync(fsql);

    Log.Information("Database seeding completed successfully!");
}
catch (Exception ex)
{
    Log.Error(ex, "Database seeding failed!");
    Environment.ExitCode = 1;
}
finally
{
    Log.CloseAndFlush();
}
