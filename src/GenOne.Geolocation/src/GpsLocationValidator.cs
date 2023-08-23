using System.Diagnostics;

namespace GenOne.Geolocation;

public static class GpsLocationValidator
{
    [StackTraceHidden]
    public static void ThrowIfInvalid(double latitude, double longitude)
    {
        ThrowIfLatitudeInvalid(latitude);
        ThrowIfLongitudeInvalid(longitude);
    }

    public static bool IsValid(double latitude, double longitude)
    {
        return IsLatitudeValid(latitude) && IsLongitudeValid(longitude);
    }

    [StackTraceHidden]
    public static void ThrowIfLatitudeInvalid(double latitude)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(latitude, GpsLocation.MinLatitude);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(latitude, GpsLocation.MaxLatitude);
    }

    public static bool IsLatitudeValid(double latitude)
    {
        return latitude is >= GpsLocation.MinLatitude and <= GpsLocation.MaxLatitude;
    }

    [StackTraceHidden]
    public static void ThrowIfLongitudeInvalid(double longitude)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(longitude, GpsLocation.MinLongitude);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(longitude, GpsLocation.MaxLongitude);
    }

    public static bool IsLongitudeValid(double longitude)
    {
        return longitude is >= GpsLocation.MinLongitude and <= GpsLocation.MaxLongitude;
    }
}