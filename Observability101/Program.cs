using Observability101.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Observability101.Database;

var builder = WebApplication.CreateBuilder(args);

// Add OpenTelemetry configuration
builder.AddObservability();

// Add Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add FastEndpoints
builder.Services.AddFastEndpoints().SwaggerDocument();

// Add HttpClient (for IHttpClientFactory)
builder.Services.AddHttpClient();

// Add Swashbuckle Swagger services
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Observability101 API", Version = "v1" });
});

// Build the application
var app = builder.Build();


// Make sure the database is always deleted and re-created with the correct schema.
// This is only for demo purposes. Don't do this in production!
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}

// For demo purposes, Scaler and OpenApi are always enabled. Don't do this in production!
app.MapOpenApi();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Observability101 API v1");
    c.RoutePrefix = "swagger";
});

// Configure the HTTP request pipeline.
app.UseFastEndpoints().UseSwaggerGen();

// Run the application
app.Run();