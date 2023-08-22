using GenOne.DPBlazorMapLibrary.Factorys.Interfaces;
using GenOne.DPBlazorMapLibrary.Models.Basics;
using GenOne.DPBlazorMapLibrary.Models.Basics.Icons;
using GenOne.DPBlazorMapLibrary.Models.Layers.Markers;

namespace GenOne.Blazor.Map
{
    internal static class IconFactoryExtensions
    {
        internal static async Task<MarkerOptions> PrepareMarkerOptions(this IIconFactory iconFactory)
        {
            IconOptions iconOptions = new()
            {
                IconUrl = "map/map-pin-fill.svg",
                IconSize = new Point(60, 60),
                IconAnchor = new Point(30, 55)
            };

            return new MarkerOptions
            {
                Opacity = 1,
                Draggable = true,
                IconRef = await iconFactory.Create(iconOptions),
                AutoPan = true,
                AutoPanPadding = new Point(100, 150)
            };
        }

        internal static async Task<MarkerOptions> PrepareUserMarkerOptions(this IIconFactory iconFactory)
        {
            DivIconOptions iconOptions = new()
            {
                ClassName = "leaflet-control-user-location",
                Html = PinsRaw.UserPinHtml,
                IconSize = new Point(24, 24),
                IconAnchor = new Point(12, 12)
            };

            return new MarkerOptions
            {
                Opacity = 1,
                Draggable = false,
                IconRef = await iconFactory.CreateDivIcon(iconOptions),
            };
        }
    }
}
