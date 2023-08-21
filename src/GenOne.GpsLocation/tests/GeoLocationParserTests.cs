using System.Globalization;
using GenOne.Geolocation;

namespace GenOne.GeoLocation.Tests;

public class GeoLocationParserTests
{
    private const string Separators = ";|,:";
    
    [Theory]
    [InlineData(50.1, 0)]
    [InlineData(12.21341441, 65.02134532)]
    [InlineData(0, 0)]
    public void ParseSuccessful(double latitude, double longitude)
    {
        foreach (var separator in Separators)
        {
            var str = $"{latitude.ToString(CultureInfo.InvariantCulture)}{separator}{longitude.ToString(CultureInfo.InvariantCulture)}";

            var data = GpsLocationParser.Parse(str, separator);
        
            Assert.Equal(latitude, data.Latitude);
            Assert.Equal(longitude, data.Longitude);
        }
    }
    
    [Theory]
    [InlineData(500, 10000)]
    [InlineData(91, 0)]
    [InlineData(-91, 0)]
    [InlineData(0, 181)]
    [InlineData(0, -181)]
    public void ParseComma_LocationsOutOfRange_Throws(double latitude, double longitude)
    {
        foreach (var separator in Separators)
        {
            var str = $"{latitude.ToString(CultureInfo.InvariantCulture)}{separator}{longitude.ToString(CultureInfo.InvariantCulture)}";
            
            Assert.Throws<ArgumentOutOfRangeException>(() => GpsLocationParser.Parse(str, separator));
        }
    }
    
    
    [Theory]
    [InlineData(50.1, 0)]
    [InlineData(12.21341441, 65.02134532)]
    [InlineData(0, 0)]
    public void TryParseSuccessful(double latitude, double longitude)
    {
        foreach (var separator in Separators)
        {
            var str = $"{latitude.ToString(CultureInfo.InvariantCulture)}{separator}{longitude.ToString(CultureInfo.InvariantCulture)}";

            var isSuccess = GpsLocationParser.TryParse(str, separator, out var data);
        
            Assert.True(isSuccess);
            Assert.Equal(latitude, data.Latitude);
            Assert.Equal(longitude, data.Longitude);
        }
    }
    
    [Theory]
    [InlineData(500, 10000)]
    [InlineData(91, 0)]
    [InlineData(-91, 0)]
    [InlineData(0, 181)]
    [InlineData(0, -181)]
    public void TryParse_LocationsOutOfRange_ReturnFalse(double latitude, double longitude)
    {
        foreach (var separator in Separators)
        {
            var str = $"{latitude.ToString(CultureInfo.InvariantCulture)}{separator}{longitude.ToString(CultureInfo.InvariantCulture)}";

            var isSuccess = GpsLocationParser.TryParse(str, separator, out var data);
        
            Assert.False(isSuccess);
        }
    }
}
