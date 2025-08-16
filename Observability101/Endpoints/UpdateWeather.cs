// ReSharper disable PropertyCanBeMadeInitOnly.Global

using Microsoft.EntityFrameworkCore;
using Observability101.Extensions;
using Observability101.Infrastructure.Database;
using Observability101.Infrastructure.Devices;

namespace Observability101.Endpoints;

public class UpdateWeatherRequest
{
    public required string City { get; set; }
}

public class UpdateWeatherResponse
{
    public required string City { get; set; }
    public required DateTime TimeStamp { get; set; }
    public decimal Temperature { get; set; }
}

public class UpdateWeather(ApplicationDbContext context, ITemperatureSensorReader reader)
    : Endpoint<UpdateWeatherRequest, UpdateWeatherResponse>
{
    public override void Configure()
    {
        Post("/api/weather");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the process of updating weather data for a specific city, ensuring the data is stored in the database,
    /// and returns the updated information.
    /// </summary>
    public override async Task HandleAsync(UpdateWeatherRequest req, CancellationToken ct)
    {
        // Truncate to minute, to only allow one temperature reading per minute 
        var normalizedNow = DateTime.UtcNow.TruncateTimeToMinute();

        // Check if the temperature for the specified city and timestamp is already in the database
        var temperature = await context.Weather
            .Where(q => q.City == req.City && q.Timestamp == normalizedNow)
            .Select(q => (decimal?)q.Temperature)
            .FirstOrDefaultAsync(ct);

        // If the temperature is null, use the sensor reader (fake) to read the temperature and save it in the db
        if (temperature is null)
        {
            temperature = await reader.ReadTemperatureAsync(req.City, ct);
            await context.Weather.AddAsync(new Weather
            {
                City = req.City,
                Timestamp = normalizedNow,
                Temperature = temperature.Value
            }, ct);
            await context.SaveChangesAsync(ct);
        }

        await Send.OkAsync(
            new UpdateWeatherResponse
            {
                City = req.City,
                TimeStamp = normalizedNow,
                Temperature = temperature.Value,
            },
            ct);
    }
}