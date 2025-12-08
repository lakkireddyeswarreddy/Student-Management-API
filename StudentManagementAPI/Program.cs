using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using StudentManagementAPI.ApplicationDbContext;
using StudentManagementAPI.CustomFilters;
using StudentManagementAPI.CustomMiddleware;
using StudentManagementAPI.Repository;
using StudentManagementAPI.RepositoryInterface;
using StudentManagementAPI.ServiceInterface;
using StudentManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j} {Scopes}{NewLine}{Exception}")
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
        .WriteTo.File("logs/info.log",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message:lj} {Properties:j} {Scopes}{NewLine}{Exception}")
    )
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
        .WriteTo.File("logs/error.log",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message:lj} {Properties:j} {Scopes}{NewLine}{Exception}")
    )
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
        .WriteTo.File("logs/debug.log",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message:lj} {Properties:j} {Scopes}{NewLine}{Exception}")
    )
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true,
        })
    .CreateLogger();



// Add services to the container.
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    c.SupportNonNullableReferenceTypes();
    c.UseAllOfToExtendReferenceSchemas();
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExecutionTimeFilter>();
    options.Filters.Add<CustomExceptionFilter>();

}).AddNewtonsoftJson();

builder.Services.AddScoped<ModelValidationFilter>();

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole(options =>
//{
//    options.FormatterName = ConsoleFormatterNames.Simple;
//});

//builder.Services.Configure<SimpleConsoleFormatterOptions>(options =>
//{
//    options.IncludeScopes = true;
//});

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<LoggingMiddleware>();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
