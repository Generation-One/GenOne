using System.Globalization;
using GenOne.Geolocation;

namespace GenOne.GeoLocation.Tests;

public class GpsLocationTests
{
    [Theory]
    [InlineData(500, 10000)]
    [InlineData(91, 0)]
    [InlineData(-91, 0)]
    [InlineData(0, 181)]
    [InlineData(0, -181)]
    public void ThrowIfCoordinatesOutOfRange(double latitude, double longitude)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GpsLocation(latitude, longitude));
    }
    
    [Theory]
    [InlineData(50.1, 0)]
    [InlineData(12.21341441, 65.02134532)]
    [InlineData(0, 0)]
    public void LocationWithSameDataAreEqual(double latitude, double longitude)
    {
        var location1 = new GpsLocation(latitude, longitude);
        var location2 = new GpsLocation(latitude, longitude);
        
        Assert.Equal(location1, location2);
    }

    [Theory]
    [InlineData(50.1, 0)]
    [InlineData(12.21341441, 65.02134532)]
    [InlineData(0, 0)]
    public void ParseCommaSuccessful(double latitude, double longitude)
    {
        var str = $"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";

        var data = GpsLocation.ParseComma(str);
        
        Assert.Equal(latitude, data.Latitude);
        Assert.Equal(longitude, data.Longitude);
    }
    
    [Theory]
    [InlineData(500, 10000)]
    [InlineData(91, 0)]
    [InlineData(-91, 0)]
    [InlineData(0, 181)]
    [InlineData(0, -181)]
    public void ParseComma_LocationsOutOfRange_Throws(double latitude, double longitude)
    {
        var str = $"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";
        
        Assert.Throws<ArgumentOutOfRangeException>(() => GpsLocation.ParseComma(str));
    }
    
    [Theory]
    [InlineData(50.1, 0)]
    [InlineData(12.21341441, 65.02134532)]
    [InlineData(0, 0)]
    public void TryParseCommaSuccessful(double latitude, double longitude)
    {
        var str = $"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";

        var isSuccess = GpsLocation.TryParseComma(str, out var data);
        
        Assert.True(isSuccess);
        Assert.Equal(latitude, data.Latitude);
        Assert.Equal(longitude, data.Longitude);
    }
    
    [Theory]
    [InlineData(500, 10000)]
    [InlineData(91, 0)]
    [InlineData(-91, 0)]
    [InlineData(0, 181)]
    [InlineData(0, -181)]
    public void TryParseComma_LocationsOutOfRange_ReturnFalse(double latitude, double longitude)
    {
        var str = $"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";
        
        var isSuccess = GpsLocation.TryParseComma(str, out _);
        Assert.False(isSuccess);
    }
}
