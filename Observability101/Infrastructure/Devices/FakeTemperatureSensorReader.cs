namespace Observability101.Infrastructure.Devices;

internal class FakeTemperatureSensorReader : ITemperatureSensorReader
{
    public async Task<decimal> ReadTemperatureAsync(string city, CancellationToken ct)
    {
        // Simulates latency for connecting to a temperature sensor
        await Task.Delay(50, ct);

        // Simulates latency for reading the value of the temperature sensor
        await Task.Delay(20, ct);
        // Simulate a random temperature reading
        var temperature = Random.Shared.Next(-20, 40);
        return temperature;
    }
}