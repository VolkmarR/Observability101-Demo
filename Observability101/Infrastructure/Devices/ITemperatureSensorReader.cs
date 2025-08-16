namespace Observability101.Infrastructure.Devices;

public interface ITemperatureSensorReader
{
    Task<decimal> ReadTemperatureAsync(string city, CancellationToken ct);
}