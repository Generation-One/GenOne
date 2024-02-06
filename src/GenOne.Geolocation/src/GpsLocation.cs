using System.Diagnostics.CodeAnalysis;

namespace GenOne.Geolocation;

public record GpsLocation
{
    public const double MinLatitude = -90;
    public const double MaxLatitude = 90;
    public const double MinLongitude = -180;
    public const double MaxLongitude = 180;

    public static GpsLocation Empty { get; } = new(0, 0);

    public double Longitude { get; } = double.MinValue;

    public double Latitude { get; } = double.MinValue;

    public GpsLocation(double latitude, double longitude)
    {
        GpsLocationValidator.ThrowIfInvalid(latitude, longitude);

        Latitude = latitude;
        Longitude = longitude;
    }

    [Obsolete("Use Parse")]
    public static GpsLocation ParseComma(string locationString)
    {
        return GpsLocationParser.Parse(locationString, ',');
    }

    [Obsolete("Use TryParse")]
    public static bool TryParseComma(string locationString, [NotNullWhen(true)] out GpsLocation? gpsLocation)
    {
        return GpsLocationParser.TryParse(locationString, ',', out gpsLocation);
    }

    public static GpsLocation Parse(string locationString, char separator)
    {
        return GpsLocationParser.Parse(locationString, separator);
    }

    public static bool TryParse(string locationString, char separator, [NotNullWhen(true)] out GpsLocation? gpsLocation)
    {
        return GpsLocationParser.TryParse(locationString, separator, out gpsLocation);
    }

    public virtual bool Equals(GpsLocation? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return Longitude.Equals(other.Longitude)
            && Latitude.Equals(other.Latitude);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Longitude, Latitude);
    }

    public override string ToString() => $"GPS:{Math.Round(Longitude, 3)},{Math.Round(Latitude, 3)}";
}