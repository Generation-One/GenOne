using GenOne.DPBlazorMapLibrary.Models.Layers.Markers;

namespace GenOne.Blazor.Map;

//todo better to split factories and use delegates
public interface IMapIconFactory
{
	Task<MarkerOptions> PrepareMarkerOptions(bool draggable);

	Task<MarkerOptions> PrepareUserMarkerOptions();
}