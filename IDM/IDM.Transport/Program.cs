using IDM.Infrastructure;
using IDM.Infrastructure.Jobs;
using IDM.Infrastructure.Options.EndpointOptions;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Quartz;

/*Ctrl + T — получить последние изменения.
    Работаешь с кодом.
    Ctrl + K — Commit.
    Пишешь осмысленное сообщение, например:
Add DepartmentSynchronizationService
Refactor IdmLoadingJob
Implement department tree update
Ctrl + Shift + K — Push.*/

var builder = WebApplication.CreateBuilder(args);

#region ПОДКЛЮЧАЕМ БД

// Получим строку подключения
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
    throw new ArgumentNullException(nameof(connectionString));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
        // включает более детальный вывод ошибок самого EF Core
        .EnableDetailedErrors()
        // включает вывод приватных данных приложения 
        // (таких, как сгенерированные строки запроса, параметры этих строк запроса)
        .EnableSensitiveDataLogging(builder.Environment.IsDevelopment()));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#endregion

#region Регистрируем эндпойнты загрузки IDM

builder.Services.Configure<EndpointOptions>(
    builder.Configuration.GetSection(nameof(EndpointOptions)));

#endregion

#region Quartz

builder.Services.AddQuartz(options =>
{
    var jobKey = new JobKey("IdmLoading", "Idm");

    options.AddJob<IdmLoadingJob>(jobKey);

    options.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("IdmLoadingJob-trigger")
        .WithSimpleSchedule(x => x
            //.WithIntervalInHours(3)
            .WithIntervalInSeconds(30)
            .RepeatForever()));
});


builder.Services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; }); /**/

#endregion

builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(p => { p.Theme = ScalarTheme.Moon; });
}

app.UseHttpsRedirection();

/*app.MapGet("/loadidm", async (IIdmLoadingService service) =>
    {
        var departments = await service.LoadDepartmentsAsync();
        return Results.Ok(departments);
    })
    .WithName("LoadIdm");*/

/*app.MapGet("/test", async ([FromServices] IIdmSynchronizationService service) =>
{
    await service.SynchronizeAsync();
    return Results.Ok();
});*/

app.MapGet("/heart", () => Results.Ok(new
    {
        status = "OK",
        timestamp = DateTime.UtcNow
    }))
    .WithName("Heart")
    .WithSummary("Checks that the application is running.");

app.MapGet("/heartbeat", async (AppDbContext db) =>
    {
        var dbAvailable = await db.Database.CanConnectAsync();

        return dbAvailable
            ? Results.Ok(new
            {
                status = "OK",
                database = "Connected",
                timestamp = DateTime.UtcNow
            })
            : Results.Problem(
                title: "Database unavailable",
                statusCode: StatusCodes.Status503ServiceUnavailable);
    })
    .WithName("Heartbeat");

app.Run();