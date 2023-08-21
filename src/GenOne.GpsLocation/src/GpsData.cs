namespace GenOne.Geolocation
{
    public record GpsData(
        GpsLocation Location,
        double PositionAccuracy,
        DateTimeOffset Timestamp,
        double Heading,
        double HeadingAccuracy,
        double Altitude,
        double Speed,
        double SpeedAccuracy) : IHasGpsLocation;
}