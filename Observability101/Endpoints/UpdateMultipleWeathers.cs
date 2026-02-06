// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Observability101.Endpoints;

public class UpdateMultipleWeathersRequest
{
    public required string[] Cities { get; set; }
}

public class UpdateMultipleWeatherResponse
{
    public required UpdateMultipleWeathersResponseItem[] Weathers { get; set; }
}

public class UpdateMultipleWeathersResponseItem
{
    public required string City { get; set; }
    public required DateTime TimeStamp { get; set; }
    public decimal Temperature { get; set; }
}

public class UpdateMultipleWeathers(IHttpClientFactory httpClientFactory)
    : Endpoint<UpdateMultipleWeathersRequest, UpdateMultipleWeatherResponse>
{
    public override void Configure()
    {
        Post("/api/weather/multiple");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the request to update weather information for multiple cities.
    /// </summary>
    public override async Task HandleAsync(UpdateMultipleWeathersRequest req, CancellationToken ct)
    {
        var results = await UpdateWeathersSimple(req.Cities, ct);
        // var results = await UpdateWeathersOptimized(req.Cities, ct);

        await Send.OkAsync(new UpdateMultipleWeatherResponse
        {
            Weathers = results
        }, cancellation: ct);
    }

    /// <summary>
    /// Simple approach to read multiple temperatures (foreach)
    /// </summary>
    /// <param name="cities">Array containing the cities</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    private async Task<UpdateMultipleWeathersResponseItem[]> UpdateWeathersSimple(string[] cities, CancellationToken ct)
    {
        var results = new List<UpdateMultipleWeathersResponseItem>();
        foreach (var city in cities)
            results.Add(await UpdateSingleWeather(city, ct));

        return results.ToArray();
    }


    /// <summary>
    /// Optimized approach to read multiple temperatures (parallel instead of sequential execution)
    /// </summary>
    /// <param name="cities">Array containing the cities</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    private async Task<UpdateMultipleWeathersResponseItem[]> UpdateWeathersOptimized(string[] cities,
        CancellationToken ct)
    {
        var tasks = cities.Select(city => UpdateSingleWeather(city, ct));
        return await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Updates the weather information for a specific city.
    /// </summary>
    /// <param name="city">The name of the city for which the weather information is to be updated.</param>
    /// <param name="ct">The cancellation token to handle task cancellation.</param>
    /// <returns>Returns the updated weather information for the specified city.</returns>
    private async Task<UpdateMultipleWeathersResponseItem> UpdateSingleWeather(string city, CancellationToken ct)
    {
        var client = httpClientFactory.CreateClient();
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