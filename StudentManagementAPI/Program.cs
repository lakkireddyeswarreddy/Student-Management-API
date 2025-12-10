using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using StudentManagementAPI.ApplicationDbContext;
using StudentManagementAPI.BackgroundServices;
using StudentManagementAPI.CustomFilters;
using StudentManagementAPI.CustomMiddleware;
using StudentManagementAPI.Model;
using StudentManagementAPI.Repository;
using StudentManagementAPI.RepositoryInterface;
using StudentManagementAPI.Service;
using StudentManagementAPI.ServiceInterface;
using StudentManagementAPI.Services;
using System.Security.Claims;
using System.Text;

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
builder.Services.AddScoped<IPasswordHasher<Teacher>, PasswordHasher<Teacher>>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
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

var jwtKey = builder.Configuration["Jwt:Key"];

var jwtBytes = Encoding.UTF8.GetBytes(jwtKey);

var jwtIssuer = builder.Configuration["Jwt:Issuer"];

//var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtIssuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtBytes)
    };

});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IITMTeacherOnly", policy =>
    {
        policy.RequireAssertion(context =>
        {
            var emailClaim = context.User.FindFirst(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null) return false;

            return emailClaim.Value.EndsWith("iitm.ac.in", StringComparison.OrdinalIgnoreCase);
        });
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader(),
        new HeaderApiVersionReader()
        );

}).AddMvc();

var MyAllowSpecificOrigins = "myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

    });
});

builder.Services.AddHostedService<CurrentTimeService>();

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

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
