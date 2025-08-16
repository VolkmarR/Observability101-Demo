using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Observability101.OpenTelemetry;

public static class TelemetryConfiguration
{
    private const string ServiceName = "Observability101";
    private const string ServiceVersion = "1.0.0";

    /// <summary>
    /// Adds OpenTelemetry configuration to the application.
    /// </summary>
    /// <param name="builder">The application builder instance.</param>
    /// <returns>The updated application builder instance with OpenTelemetry configuration added.</returns>
    public static IHostApplicationBuilder AddObservability(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resourceBuilder =>
                {
                    resourceBuilder.AddService(ServiceName, serviceVersion: ServiceVersion,
                        serviceInstanceId: builder.Environment.EnvironmentName);
                    resourceBuilder.AddAttributes(new Dictionary<string, object>
                    {
                        ["host.name"] = Environment.MachineName
                    });
                }
            )
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddNpgsql()
                    .AddCustomTracing();
            });

        if (!string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
            builder.Services.AddOpenTelemetry().UseOtlpExporter();

        return builder;
    }
}