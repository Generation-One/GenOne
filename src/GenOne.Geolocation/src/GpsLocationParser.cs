using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GenOne.Geolocation;

internal static class GpsLocationParser
{
    public static GpsLocation Parse(string locationString, char separator)
    { 
        ArgumentNullException.ThrowIfNull(locationString);

        var i = locationString.Split(separator);
        if (i.Length != 2)
        {
            throw new FormatException($"Wrong location string format: {locationString}");
        }

        return new GpsLocation(double.Parse(i[0], CultureInfo.InvariantCulture), double.Parse(i[1], CultureInfo.InvariantCulture));
    }
        
    public static bool TryParse([NotNullWhen(true)] string? locationString, char separator, out GpsLocation gpsLocation)
    {
        if (locationString is null)
        {
            gpsLocation = default;
            return false;
        }
            
        var i = locationString.Split(separator);
        if (i.Length != 2)
        {
            gpsLocation = default;
            return false;
        }

        if (!double.TryParse(i[0], CultureInfo.InvariantCulture, out var latitude) 
            || !GpsLocationValidator.IsLatitudeValid(latitude))
        {
            gpsLocation = default;
            return false;
        }

        if (!double.TryParse(i[1], CultureInfo.InvariantCulture, out var longitude) 
            || !GpsLocationValidator.IsLongitudeValid(longitude))
        {
            gpsLocation = default;
            return false;
        }
            
        gpsLocation = new GpsLocation(latitude, longitude);
        return true;
    }
}