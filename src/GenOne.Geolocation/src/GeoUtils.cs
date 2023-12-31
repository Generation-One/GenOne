﻿namespace GenOne.Geolocation;

public static class GeoUtils
{
    public const double EarthRadiusKm = 6371;
    public const double EarthRadiusM = EarthRadiusKm * 1000;
    public const double PI180 = Math.PI / 180.0;
    public const double DEGREE_TO_KM = 111.1111;
    public const double KM_TO_DEGREE = 1.0 / DEGREE_TO_KM;

    public static double ToRadians(this double degrees) => degrees * PI180;

    /// <summary>
    /// https://stackoverflow.com/a/51839058
    /// </summary>
    /// <param name="longitude"></param>
    /// <param name="latitude"></param>
    /// <param name="otherLongitude"></param>
    /// <param name="otherLatitude"></param>
    /// <returns></returns>
    public static double GetDistanceInMeters(double latitude, double longitude, double otherLatitude, double otherLongitude)
    {
        var d1 = latitude * PI180;
        var num1 = longitude * PI180;
        var d2 = otherLatitude * PI180;
        var num2 = otherLongitude * PI180 - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        return EarthRadiusM * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
    }

    public static double GetDistanceInMeters(GpsLocation location, GpsLocation otherLocation)
    {
        ArgumentNullException.ThrowIfNull(location);
        ArgumentNullException.ThrowIfNull(otherLocation);

        return GetDistanceInMeters(location.Latitude, location.Longitude, otherLocation.Latitude, otherLocation.Longitude);
    }

    public static GeoBounds GetBounds(double latitude, double longitude, double distanceKm)
    {
        var dLatitude = distanceKm * KM_TO_DEGREE;
        var dLongitude = dLatitude / Math.Cos(latitude);

        return new GeoBounds(
            Simplify(latitude - dLatitude, GpsLocation.MaxLatitude),
            Simplify(longitude - dLatitude, GpsLocation.MaxLongitude),
            Simplify(latitude + dLongitude, GpsLocation.MaxLatitude),
            Simplify(longitude + dLongitude, GpsLocation.MaxLongitude)
        );

        static double Simplify(double val, double factor)
        {
            if (val > factor)
                return val - factor * 2;
            
            if (val < -factor)
                return val + factor * 2;

            return val;
        }
    }

    /// <summary>
    /// Check if two points are within x meters
    /// </summary>
    /// <param name="gpsLocation"></param>
    /// <param name="secondLocation"></param>
    /// <param name="rangeInMeters"></param>
    /// <returns></returns>
    public static bool IsProximity(this GpsLocation gpsLocation, GpsLocation secondLocation, int rangeInMeters)
    {
        ArgumentNullException.ThrowIfNull(gpsLocation);
        ArgumentNullException.ThrowIfNull(secondLocation);

        var distanceMeters = GetDistanceInMeters(gpsLocation, secondLocation);
        return distanceMeters <= rangeInMeters;
    }
}