using System.Threading.Channels;
using GenOne.Geolocation;

namespace GenOne.Blazor.Map.Component
{
    public partial class Map : IDisposable
    {
        private readonly Channel<FieldUpdated> _updates = Channel.CreateUnbounded<FieldUpdated>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        public void Dispose()
        {
            _updates.Writer.Complete();
        }

        private void SubscribeToConsumerLocationUpdates()
        {
            var locationProvider = (IUserLocationProvider?)Services.GetService(typeof(IUserLocationProvider));
            locationProvider?.Subscribe(x => ConsumerLocation = x);
        }

        private void StartReadUpdates()
        {
            async Task Read()
            {
                await foreach (var item in _updates.Reader.ReadAllAsync())
                {
                    try
                    {
                        switch (item)
                        {
                            case FieldUpdated.ConsumerLocation:
                                _userMarker = await Factory.AddOrUpdateMarker(_userMarker, _map, _consumerLocation?.Location.LatLng(), ResolvedIconFactory.PrepareUserMarkerOptions);
                                if (FollowConsumerLocation)
                                {
                                    await UpdateViewLocation(_consumerLocation?.Location);
                                }
                                break;

                            case FieldUpdated.MarkerLocationCanBeChanged:
                                var prevMarker = _marker;
                                _marker = await Factory.AddOrUpdateMarker(null, _map, _markerPosition.LatLng(), () => ResolvedIconFactory.PrepareMarkerOptions(MarkerLocationCanBeChanged),
                                    async _ =>
                                    {
                                        _markerPosition = 
                                            _marker is not null 
                                                ? (await _marker.GetLatLng()).GpsLocation() 
                                                : null;

                                        await MarkerPositionChanged.InvokeAsync(_markerPosition);
                                    });

                                if (prevMarker is not null)
                                {
                                    await prevMarker.Remove();
                                }
                                break;

                            case FieldUpdated.MarkerLocation:
                                _marker = await Factory.AddOrUpdateMarker(_marker, _map, _markerPosition.LatLng(), () => ResolvedIconFactory.PrepareMarkerOptions(MarkerLocationCanBeChanged),
                                    async x =>
                                    {
                                        _markerPosition =
                                            _marker is not null
                                                ? (await _marker.GetLatLng()).GpsLocation()
                                                : null;

                                        await MarkerPositionChanged.InvokeAsync(_markerPosition);
                                    });
                                break;

                            case FieldUpdated.FollowConsumerLocation:
                                if (FollowConsumerLocation)
                                {
                                    await UpdateViewLocation(_consumerLocation?.Location);
                                }
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    
                }
            }

            Task.Run(Read, CancellationToken.None);
        }

        public async Task ViewAllMarkers(bool flyTo = true, int? zoom = null)
        {
            if(_map is null)
                return;

            switch (MarkerPosition is not null, ConsumerLocation is not null)
            {
                case (true, true):
	                var bounds = GeoBounds.FromLocations(MarkerPosition!, ConsumerLocation!.Location).LatLngBounds();
                    if (flyTo)
                    { 
                        await _map.FlyToBounds(bounds);
                    }
                    else
                    {
                        await _map.FitBounds(bounds);
                    }
                    break;

                case (true, false):
	                await UpdateViewLocation(MarkerPosition, flyTo, zoom);
	                break;

                case (false, true):
	                await UpdateViewLocation(ConsumerLocation?.Location, flyTo, zoom);
	                break;
			}
        }

		private async Task UpdateViewLocation(GpsLocation? location, bool flyTo = true, int? zoom = null)
        {
            var latLng = location.LatLng();

            if (latLng is null || _map is null)
                return;

            if (flyTo)
            {
                await _map.FlyTo(latLng, zoom ?? Zoom);
            } else
            {
                await _map.SetView(latLng, zoom ?? Zoom);
            }
        }

        private IMapIconFactory ResolvedIconFactory => OverrideIconFactory ?? IconFactory;

        private enum FieldUpdated
        {
            ConsumerLocation,
            MarkerLocation,
            MarkerLocationCanBeChanged,
            FollowConsumerLocation
        }
    }
}
