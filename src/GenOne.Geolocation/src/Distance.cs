namespace GenOne.Geolocation;

public readonly struct Distance : IComparable<Distance>, IEquatable<Distance>
{
    private const double MetersPerKilometer = 1000.0;
    public Distance(double meters)
    {
        Meters = meters;
    }

    public double Meters { get; }

    public double Kilometers => Meters / MetersPerKilometer;

    public static Distance FromMeters(double meters)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(meters, 0);
        
        return new Distance(meters);
    }

    public static Distance FromKilometers(double kilometers)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(kilometers, 0);

        return new Distance(kilometers * MetersPerKilometer);
    }

    public static Distance BetweenPositions(GpsLocation position1, GpsLocation position2)
    {
        var distanceInMeters = GeoUtils.GetDistanceInMeters(position1, position2);
        return FromMeters(distanceInMeters);
    }

    public bool Equals(Distance other)
    {
        return Meters.Equals(other.Meters);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        return obj is Distance distance && Equals(distance);
    }

    public override int GetHashCode()
    {
        return Meters.GetHashCode();
    }

    public static bool operator ==(Distance left, Distance right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Distance left, Distance right)
    {
        return !left.Equals(right);
    }
    
    public static bool operator <(Distance left, Distance right)
    {
        return left.CompareTo(right) < 0;
    }
    
    public static bool operator >(Distance left, Distance right)
    {
        return left.CompareTo(right) > 0;
    }
    
    public static bool operator <=(Distance left, Distance right)
    {
        return left.CompareTo(right) <= 0;
    }
    
    public static bool operator >=(Distance left, Distance right)
    {
        return left.CompareTo(right) >= 0;
    }

    public int CompareTo(Distance other)
    {
        return Meters.CompareTo(other.Meters);
    }
}
