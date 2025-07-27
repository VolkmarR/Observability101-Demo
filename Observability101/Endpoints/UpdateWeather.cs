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

public class UpdateWeather : Endpoint<UpdateWeatherRequest, UpdateWeatherResponse>
{
    public override void Configure()
    {
        Post("/api/weather");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateWeatherRequest req, CancellationToken ct)
    {
        var now = DateTime.Now;
        var normalizedNow = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
        
        await Send.OkAsync(new()
            {
                City = req.City, TimeStamp = normalizedNow, Temperature = 20.5m,
            }
            , ct);
    }
}