using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

// OpenTelemetry Metrics: ASP.NET Core + Runtime (GC, ThreadPool, LOH, Exceptions)
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()   // HTTP запросы/ответы
            .AddRuntimeInstrumentation()      // GC, ThreadPool, исключения и др.
            .AddPrometheusExporter();         // Экспортер на /metrics
    });

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true
});

app.MapPrometheusScrapingEndpoint("/metrics");

app.MapGet("/", () => Results.Text("OK"));

app.Run();
