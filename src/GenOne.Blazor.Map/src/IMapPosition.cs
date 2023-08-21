
namespace GenOne.Blazor.Map
{
    public interface IMapPosition
    {
        double Zoom { get; }
    }

    public record MapPosition(double Latitude, double Longitude, double Zoom) : IMapPosition
    {
        public bool IsEmpty => false;
    }
}