using System.Diagnostics.CodeAnalysis;
using GenOne.DPBlazorMapLibrary.Models.Basics;
using GenOne.Geolocation;

namespace GenOne.Blazor.Map
{
    internal static class MapHelpers
    {
        [return: NotNullIfNotNull(nameof(latLng))]
        public static GpsLocation? GpsLocation(this LatLng? latLng)
        {
            return latLng is null 
                ? null
                : new GpsLocation(latLng.Lat, latLng.Lng);
        }

        [return: NotNullIfNotNull(nameof(location))]
        public static LatLng? LatLng(this GpsLocation? location)
        {
            return location is null
				? null
				: new LatLng(location.Latitude, location.Longitude);
        }

		[return: NotNullIfNotNull(nameof(bounds))]
		public static LatLngBounds? LatLngBounds(this GeoBounds? bounds)
        {
	        return bounds is null
		        ? default
		        : new LatLngBounds(bounds.LocationMin.LatLng(), bounds.LocationMax.LatLng());
		}

	}

    public interface IUserLocationProvider : IObservable<GpsData>
    { }
}