using System.Diagnostics.CodeAnalysis;

namespace GenOne.Geolocation;

public record GpsLocation(double Latitude, double Longitude)
{
    public const double MinLatitude = -90;
    public const double MaxLatitude = 90;
    public const double MinLongitude = -180;
    public const double MaxLongitude = 180;

    public static GpsLocation Empty { get; } = new(0, 0);

    [Obsolete("Use Parse")]
    public static GpsLocation ParseComma(string locationString)
    {
        return GpsLocationParser.Parse(locationString, ',');
    }

    [Obsolete("Use TryParse")]
    public static bool TryParseComma([NotNullWhen(true)] string? locationString, out GpsLocation gpsLocation)
    {
        return GpsLocationParser.TryParse(locationString, ',', out gpsLocation);
    }

    public static GpsLocation Parse(string locationString, char separator)
    {
        return GpsLocationParser.Parse(locationString, separator);
    }

    public static bool TryParse([NotNullWhen(true)] string? locationString, char separator, out GpsLocation gpsLocation)
    {
        return GpsLocationParser.TryParse(locationString, separator, out gpsLocation);
    }

    public virtual bool Equals(GpsLocation? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude);
    }
}