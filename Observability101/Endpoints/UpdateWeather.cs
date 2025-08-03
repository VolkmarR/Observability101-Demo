using Microsoft.EntityFrameworkCore;
using Observability101.Database;
using Observability101.Extensions;

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

public class UpdateWeather(ApplicationDbContext context) : Endpoint<UpdateWeatherRequest, UpdateWeatherResponse>
{
    public override void Configure()
    {
        Post("/api/weather");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateWeatherRequest req, CancellationToken ct)
    {
        var normalizedNow = DateTime.UtcNow.TruncateTimeToMinute();

        var temperature = await context.Weather
            .Where(q => q.City == req.City && q.Timestamp == normalizedNow)
            .Select(q => (decimal?)q.Temperature)
            .FirstOrDefaultAsync(ct);

        if (temperature is null)
        {
            temperature = Random.Shared.Next(-20, 40);
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