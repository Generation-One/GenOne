namespace GenOne.Geolocation
{
    public record GeoBounds
    {
        public GpsLocation LocationMin { get; init; }

        public GpsLocation LocationMax { get; init; }

        public GeoBounds(double latitudeMin, double longitudeMin, double latitudeMax, double longitudeMax)
        {
            LocationMin = new GpsLocation(latitudeMin, longitudeMin);
            LocationMax = new GpsLocation(latitudeMax, longitudeMax);
        }

        public GeoBounds(GpsLocation locationMin, GpsLocation locationMax)
        {
            LocationMin = locationMin;
            LocationMax = locationMax;
        }
    }
}