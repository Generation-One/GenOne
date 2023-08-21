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
            => UserLocationProvider?.Subscribe(x => ConsumerLocation = x);

        private void StartReadUpdates()
        {
            async Task Read()
            {
                await foreach (var item in _updates.Reader.ReadAllAsync())
                {
                    switch (item)
                    {
                        case FieldUpdated.ViewPosition:
                            await UpdateViewLocation();
                            break;
                        case FieldUpdated.ConsumerLocation:
                            _marker = await Factory.AddOrUpdateMarker(_userMarker, _map, _consumerLocation?.Location.LatLng(), IconFactory.PrepareUserMarkerOptions);
                            break;
                        case FieldUpdated.MarkerLocation:
                            _marker = await Factory.AddOrUpdateMarker(_marker, _map, _markerPosition.LatLng(), IconFactory.PrepareMarkerOptions);
                            break;
                    }
                }
            }

            Task.Run(Read, CancellationToken.None);
        }
        
        public Task UpdateViewLocation(GpsLocation? location, bool flyTo = true, int? zoom = null)
        {
            _viewPosition = location;
            return UpdateViewLocation(flyTo, zoom);
        }

        private async Task UpdateViewLocation(bool flyTo = true, int? zoom = null)
        {
            var latLng = _viewPosition.LatLng();

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

        private enum FieldUpdated
        {
            ConsumerLocation,
            MarkerLocation,
            ViewPosition
        }
    }
}
