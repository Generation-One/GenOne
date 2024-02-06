using GenOne.DPBlazorMapLibrary.Models.Layers.Markers;

namespace GenOne.Blazor.Map;

public interface IMapIconFactory
{
	Task<MarkerOptions> PrepareMarkerOptions(bool draggable);

	Task<MarkerOptions> PrepareUserMarkerOptions();
}