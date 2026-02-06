using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Observability101.Infrastructure.Database;

public class Weather
{
    public int Id { get; set; }

    public string City { get; set; } = default!;

    public DateTime Timestamp { get; set; }
    public decimal Temperature { get; set; }
}

public class WeatherConfiguration : IEntityTypeConfiguration<Weather>
{
    public void Configure(EntityTypeBuilder<Weather> builder)
    {
        builder.ToTable("Weathers");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.City).HasMaxLength(100);
        builder.Property(w => w.Timestamp).IsRequired();
        builder.Property(w => w.Temperature).IsRequired();

        builder.HasIndex(w => new { w.City, w.Timestamp }, "IX_City_Timestamp");
    }
}