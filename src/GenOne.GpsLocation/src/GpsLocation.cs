﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace GenOne.Geolocation
{
    [StructLayout(LayoutKind.Auto)]
    public readonly record struct GpsLocation
    {
        public const double MinLatitude = -90;
        public const double MaxLatitude = 90;
        public const double MinLongitude = -180;
        public const double MaxLongitude = 180;

        public static GpsLocation Empty { get; } = new();

        public double Longitude { get; } = double.MinValue;

        public double Latitude { get; } = double.MinValue;

        public GpsLocation(double latitude, double longitude)
        {
            GpsLocationValidator.ThrowIfInvalid(latitude, longitude);

            Latitude = latitude;
            Longitude = longitude;
        }

        public static GpsLocation ParseComma(string locationString)
        {
            return GpsLocationParser.Parse(locationString, ',');
        }

        public static bool TryParseComma(string locationString, out GpsLocation gpsLocation)
        {
            return GpsLocationParser.TryParse(locationString, ',', out gpsLocation);
        }

        public bool Equals(GpsLocation other) 
            => Longitude.Equals(other.Longitude) && Latitude.Equals(other.Latitude);

        public override int GetHashCode() 
            => HashCode.Combine(Longitude, Latitude);

        public override string ToString() => $"GPS:{Math.Round(Longitude, 3)},{Math.Round(Latitude, 3)}";
    }
}
