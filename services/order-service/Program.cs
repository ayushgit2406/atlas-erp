using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OrderService.Application.Interfaces;
using OrderService.Application.Services;
using OrderService.Application.Validators;
using OrderService.Infrastructure.Errors;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Idempotency;
using OrderService.Infrastructure.Interfaces;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.Workers;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<OrderService.Contracts.CreateOrderRequest>, CreateOrderRequestValidator>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrdersDb")));

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]!));
builder.Services.AddScoped<IIdempotencyStore, RedisIdempotencyStore>();

builder.Services.AddScoped<IOrderService, OrderService.Application.Services.OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOutboxPublisher, OutboxPublisher>();
builder.Services.AddScoped<IInboxRepository, InboxRepository>();
builder.Services.AddHostedService<OutboxWorker>();
builder.Services.AddHostedService<OrderEventsConsumer>();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddMeter("OrderService.Events")
            .AddPrometheusExporter();
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();
app.Use(async (context, next) =>
{
    const string headerName = "X-Correlation-Id";
    if (!context.Request.Headers.TryGetValue(headerName, out var correlationId) || string.IsNullOrWhiteSpace(correlationId))
    {
        correlationId = Guid.NewGuid().ToString();
        context.Request.Headers[headerName] = correlationId;
    }

    context.Response.OnStarting(() =>
    {
        context.Response.Headers[headerName] = correlationId.ToString();
        return Task.CompletedTask;
    });

    await next();
});
app.MapControllers();
app.MapGet("/health", () => Results.Ok("ok"));
app.MapGet("/ready", async (OrdersDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return canConnect ? Results.Ok("ready") : Results.StatusCode(503);
});
app.MapPrometheusScrapingEndpoint();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
