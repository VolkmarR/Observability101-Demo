using System.Diagnostics;
using OpenTelemetry.Trace;

namespace Observability101.OpenTelemetry;

public static class CustomSourceForTracing
{
    private const string CustomTraceKey = "CustomActivity";

    /// <summary>
    /// Static ActivitySource instance used to add custom spans
    /// </summary>
    public static ActivitySource Source { get; } = new ActivitySource(CustomTraceKey, "1.0.0");

    /// <summary>
    /// Adds custom tracing support to the TracerProviderBuilder.
    /// </summary>
    /// <param name="builder">The TracerProviderBuilder instance to which the custom tracing source will be added.</param>
    /// <returns>The updated TracerProviderBuilder instance with the custom tracing source configured.</returns>
    public static TracerProviderBuilder AddCustomTracing(this TracerProviderBuilder builder)
        => builder.AddSource(CustomTraceKey);
}