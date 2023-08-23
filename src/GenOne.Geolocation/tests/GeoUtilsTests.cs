using GenOne.Geolocation;

namespace GenOne.GeoLocation.Tests;

public class GeoUtilsTests
{

    [Theory]
    [InlineData(48.366910, 135.033441, 48.350484, 135.157889, 9375)]
    [InlineData(48.427057, 135.159047, 35.908688, 14.493164, 8953816)]
    [InlineData(48.366910, 135.033441, 48.366910, 135.033441, 0)]
    [InlineData(30.898655, 137.673337, 23.531397, -112.225757, 10424196)]
    [InlineData(5.526603, 162.794363, -39.978588, -107.557199, 10372151)]
    public void GetDistanceInMeters_ReturnRightDistance(double latitude, double longitude, double otherLatitude, double otherLongitude, double distance)
    {
        const double distanceErrorInMeters = 1;
        var actual = GeoUtils.GetDistanceInMeters(latitude, longitude, otherLatitude, otherLongitude);
        Assert.InRange(actual, distance - distanceErrorInMeters, distance + distanceErrorInMeters);
    }
    
    [Theory]
    [InlineData(48.366910, 135.033441, 10)]
    [InlineData(48.366910, 135.033441, 100)]
    [InlineData(48.366910, 135.033441, 1000)]
    //[InlineData(48.366910, 135.033441, 10000)] todo fix case
    public void GetBounds_ReturnRightBounds(double latitude, double longitude, double distanceKm)
    {
        var expectedDistance = distanceKm * 1000 * 2 * Math.Sqrt(2); // Pythagorean theorem, hypotenuse = 2a * sqrt(2)
        var distanceErrorInMeters = distanceKm / 3 * 1000; //todo too big error
        
        var actual = GeoUtils.GetBounds(latitude, longitude, distanceKm);
        var distanceInMeters = GeoUtils.GetDistanceInMeters(actual.LocationMax, actual.LocationMin);
        
        Assert.InRange(distanceInMeters, expectedDistance - distanceErrorInMeters, expectedDistance + distanceErrorInMeters);
    }
}