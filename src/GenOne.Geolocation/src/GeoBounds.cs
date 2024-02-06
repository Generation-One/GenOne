namespace GenOne.Geolocation;

public record GeoBounds
{
    public GpsLocation LocationMin { get; init; }
    public GpsLocation LocationMax { get; init; }

    public GeoBounds(double latitudeMin, double longitudeMin, double latitudeMax, double longitudeMax)
    {
        LocationMin = new GpsLocation(latitudeMin, longitudeMin);
        LocationMax = new GpsLocation(latitudeMax, longitudeMax);
    }

    public static GeoBounds FromLocations(GpsLocation first, GpsLocation second)
    {
	    var (latMin, latMax) = first.Latitude < second.Latitude ? (first.Latitude, second.Latitude) : (second.Latitude, first.Latitude);
	    var (lonMin, lonMax) = first.Longitude < second.Longitude ? (first.Longitude, second.Longitude) : (second.Longitude, first.Longitude);

        return new(latMin, lonMin, latMax, lonMax);
    }

	public virtual bool Equals(GeoBounds? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return LocationMin.Equals(other.LocationMin) && LocationMax.Equals(other.LocationMax);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LocationMin, LocationMax);
    }
}