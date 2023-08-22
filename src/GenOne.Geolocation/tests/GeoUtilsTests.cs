using GenOne.Geolocation;

namespace GenOne.GeoLocation.Tests;

public class GeoUtilsTests
{
    private const double DistanceErrorInMeters = 1;

    [Theory]
    [InlineData(48.366910, 135.033441, 48.350484, 135.157889, 9375)]
    [InlineData(48.427057, 135.159047, 35.908688, 14.493164, 8953816)]
    [InlineData(30.898655, 137.673337, 23.531397, -112.225757, 10424196)]
    [InlineData(5.526603, 162.794363, -39.978588, -107.557199, 10372151)]
    public void GetDistanceInMeters_ReturnRightDistance(double latitude, double longitude, double otherLatitude, double otherLongitude, double distance)
    {
        var actual = GeoUtils.GetDistanceInMeters(latitude, longitude, otherLatitude, otherLongitude);
        Assert.InRange(actual, distance - DistanceErrorInMeters, distance + DistanceErrorInMeters);
    }
}