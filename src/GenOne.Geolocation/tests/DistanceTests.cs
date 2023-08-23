using GenOne.Geolocation;

namespace GenOne.GeoLocation.Tests;

public class DistanceTests
{
    [Theory]
    [InlineData(50, 0.05)]
    [InlineData(0, 0)]
    [InlineData(10000, 10)]
    [InlineData(1243, 1.243)]
    [InlineData(912314, 912.314)]
    public void FromMeters_CorrectValue(double meters, double kilometers)
    {
        var distance = Distance.FromMeters(meters);
        Assert.Equal(meters, distance.Meters);
        Assert.Equal(kilometers, distance.Kilometers);
    }
    
    [Theory]
    [InlineData(50, 0.05)]
    [InlineData(10000, 10)]
    [InlineData(0, 0)]
    [InlineData(1243, 1.243)]
    [InlineData(912314, 912.314)]
    public void FromKilometers_CorrectValue(double meters, double kilometers)
    {
        var distance = Distance.FromKilometers(kilometers);
        Assert.Equal(meters, distance.Meters);
        Assert.Equal(kilometers, distance.Kilometers);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(-0.0000001)]
    [InlineData(-1243)]
    [InlineData(-912314)]
    public void FromMeters_ThrowOnNegative(double meters)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Distance.FromMeters(meters));
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(-0.0000001)]
    [InlineData(-1243)]
    [InlineData(-912314)]
    public void FromKilometers_ThrowOnNegative(double kilometers)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Distance.FromKilometers(kilometers));
    }
    
    [Theory]
    [InlineData(48.366910, 135.033441, 48.350484, 135.157889, 9375)]
    [InlineData(48.366910, 135.033441, 48.366910, 135.033441, 0)]
    [InlineData(48.427057, 135.159047, 35.908688, 14.493164, 8953816)]
    [InlineData(30.898655, 137.673337, 23.531397, -112.225757, 10424196)]
    [InlineData(5.526603, 162.794363, -39.978588, -107.557199, 10372151)]
    public void BetweenPositions_CorrectResult(double latitude, double longitude, double otherLatitude, double otherLongitude, double distance)
    {
        const double distanceErrorInMeters = 1;
        
        var location = new GpsLocation(latitude, longitude);
        var otherLocation = new GpsLocation(otherLatitude, otherLongitude);
        var actual = Distance.BetweenPositions(location, otherLocation);
        
        Assert.InRange(actual.Meters, distance - distanceErrorInMeters, distance + distanceErrorInMeters);
    }
    
    [Theory]
    [InlineData(50, 40)]
    [InlineData(0.0000001, 0)]
    [InlineData(50.00000001, 50)]
    public void Comparison_LowerAndHigherValue_CorrectResult(double distance, double lowerDistance)
    {
        var higher = Distance.FromMeters(distance);
        var lower = Distance.FromMeters(lowerDistance);
        
        Assert.True(higher > lower);
        Assert.True(higher >= lower);
        Assert.True(higher.CompareTo(lower) > 0);
        Assert.True(higher != lower);
        Assert.False(higher == lower);
        
        Assert.True(lower < higher);
        Assert.True(lower <= higher);
        Assert.True(lower.CompareTo(higher) < 0);
        Assert.True(lower != higher);
        Assert.False(lower == higher);
    }
    
    [Theory]
    [InlineData(50)]
    [InlineData(0.0000001)]
    [InlineData(50.00000001)]
    public void Comparison_SameValue_CorrectResult(double distance)
    {
        var actual1 = Distance.FromMeters(distance);
        var actual2 = Distance.FromMeters(distance);
        
        Assert.False(actual1 > actual2);
        Assert.True(actual1 >= actual2);
        Assert.True(actual1.CompareTo(actual2) == 0);
        Assert.True(actual1 == actual2);
        Assert.False(actual1 != actual2);
        
        Assert.False(actual2 < actual1);
        Assert.True(actual2 <= actual1);
        Assert.True(actual2.CompareTo(actual1) == 0);
        Assert.True(actual2 == actual1);
        Assert.False(actual2 != actual1);
    }
    
    [Theory]
    [InlineData(50)]
    [InlineData(0.0000001)]
    [InlineData(50.00000001)]
    public void Comparison_WithNull_CorrectResult(double distance)
    {
        var actual = Distance.FromMeters(distance);
        
#pragma warning disable CS0464 CS8073
        Assert.False(actual > null);
        Assert.False(actual >= null);
        Assert.False(actual == null);
        Assert.True(actual != null);
        
        Assert.False(null < actual);
        Assert.False(null <= actual);
        Assert.False(null == actual);
        Assert.True(null != actual);
#pragma warning restore CS0464 CS8073
    }
}
