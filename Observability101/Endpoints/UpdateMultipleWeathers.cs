using Microsoft.AspNetCore.Http.HttpResults;

namespace Observability101.Endpoints;

public class UpdateMultipleWeathersRequest
{
    public required List<string> Cities { get; set; }
}

public class UpdateMultipleWeatherResponse
{
    public required List<UpdateMultipleWeathersResponseItem> Weathers { get; set; }
}


public class UpdateMultipleWeathersResponseItem
{
    public required string City { get; set; }
    public required DateTime TimeStamp { get; set; }
    public decimal Temperature { get; set; }
}

public class UpdateMultipleWeathers : Endpoint<UpdateMultipleWeathersRequest, UpdateMultipleWeatherResponse>
{
    private readonly HttpClient _httpClient;

    public UpdateMultipleWeathers(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override void Configure()
    {
        Post("/api/weather/multiple");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateMultipleWeathersRequest req, CancellationToken ct)
    {
        var tasks = req.Cities.Select(city => UpdateSingleWeather(city, ct));
        var results = await Task.WhenAll(tasks);

        await Send.OkAsync(new UpdateMultipleWeatherResponse
        {
            Weathers = results.ToList()
        }, cancellation: ct);
    }

    private async Task<UpdateMultipleWeathersResponseItem> UpdateSingleWeather(string city, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/weather", new UpdateWeatherRequest { City = city }, ct);
        var result = await response.Content.ReadFromJsonAsync<UpdateWeatherResponse>(cancellationToken: ct);
        return new UpdateMultipleWeathersResponseItem
        {
            City = result!.City,
            TimeStamp = result.TimeStamp,
            Temperature = result.Temperature
        };
    }
}