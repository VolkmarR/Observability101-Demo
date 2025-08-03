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
    private readonly IHttpClientFactory _httpClientFactory;

    public UpdateMultipleWeathers(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseURL);

        var response = await client.PostAsJsonAsync("/api/weather", new UpdateWeatherRequest { City = city }, ct);
        var result = await response.Content.ReadFromJsonAsync<UpdateWeatherResponse>(cancellationToken: ct);
        return new UpdateMultipleWeathersResponseItem
        {
            City = result!.City,
            TimeStamp = result.TimeStamp,
            Temperature = result.Temperature
        };
    }
}