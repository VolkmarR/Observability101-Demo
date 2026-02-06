using System.Diagnostics;
using Observability101.OpenTelemetry;

namespace Observability101.Infrastructure.Devices;

internal class FakeTemperatureSensorReader : ITemperatureSensorReader
{
    public async Task<decimal> ReadTemperatureAsync(string city, CancellationToken ct)
    {
        using var activity = CustomSourceForTracing.Source.StartActivity();

        // Simulates latency for connecting to a temperature sensor
        await Task.Delay(50, ct);

        // Add an event to the current activity
        Activity.Current?.AddEvent(new ActivityEvent("Connected to temperature sensor"));

        // Simulates latency for reading the value of the temperature sensor
        await Task.Delay(20, ct);
        // Simulate a random temperature reading
        var temperature = Random.Shared.Next(-20, 40);

        // Add an event to the current activity
        Activity.Current?.AddEvent(new ActivityEvent("Read to temperature sensor"));

        // Add custom tags to the activity with the requested city and the temperature
        Activity.Current?.AddTag("custom.city", city);
        Activity.Current?.AddTag("custom.temperature", temperature);

        return temperature;
    }
}