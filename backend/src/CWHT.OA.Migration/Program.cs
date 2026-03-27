using Microsoft.Extensions.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting database migration...");

    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    var connectionString = configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Connection string 'Default' not found.");

    var fsql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.PostgreSQL, connectionString)
        .UseAutoSyncStructure(true)
        .UseMonitorCommand(cmd => Log.Information($"SQL: {cmd.CommandText}"))
        .Build();

    Log.Information("Syncing database structure...");

    fsql.CodeFirst.SyncStructure(
        typeof(CWHT.OA.Domain.Entities.System.User),
        typeof(CWHT.OA.Domain.Entities.System.Role),
        typeof(CWHT.OA.Domain.Entities.System.UserRole),
        typeof(CWHT.OA.Domain.Entities.System.Department),
        typeof(CWHT.OA.Domain.Entities.System.Position),
        typeof(CWHT.OA.Domain.Entities.System.Menu),
        typeof(CWHT.OA.Domain.Entities.System.RoleMenu),
        typeof(CWHT.OA.Domain.Entities.System.Permission),
        typeof(CWHT.OA.Domain.Entities.System.RolePermission),
        typeof(CWHT.OA.Domain.Entities.System.DictType),
        typeof(CWHT.OA.Domain.Entities.System.DictData),
        typeof(CWHT.OA.Domain.Entities.Workflow.WorkflowDefinition),
        typeof(CWHT.OA.Domain.Entities.Workflow.WorkflowNode),
        typeof(CWHT.OA.Domain.Entities.Workflow.WorkflowTransition),
        typeof(CWHT.OA.Domain.Entities.Workflow.WorkflowInstance),
        typeof(CWHT.OA.Domain.Entities.Workflow.WorkflowTask),
        typeof(CWHT.OA.Domain.Entities.Approval.Leave),
        typeof(CWHT.OA.Domain.Entities.Approval.BusinessTrip),
        typeof(CWHT.OA.Domain.Entities.Approval.Expense),
        typeof(CWHT.OA.Domain.Entities.Approval.Reimbursement),
        typeof(CWHT.OA.Domain.Entities.Notice.Announcement),
        typeof(CWHT.OA.Domain.Entities.Notice.Message),
        typeof(CWHT.OA.Domain.Entities.Schedule.Calendar),
        typeof(CWHT.OA.Domain.Entities.Schedule.Event),
        typeof(CWHT.OA.Domain.Entities.Attendance.AttendanceRecord)
    );

    Log.Information("Database migration completed successfully!");
}
catch (Exception ex)
{
    Log.Error(ex, "Database migration failed!");
    Environment.ExitCode = 1;
}
finally
{
    Log.CloseAndFlush();
}
