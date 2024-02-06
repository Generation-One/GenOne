using GenOne.DPBlazorMapLibrary.Models.Events;

namespace GenOne.Blazor.Map;

public static class EventedExtensions
{
    public static Task OnDragEnd(this Evented evented, Func<MouseEvent, Task> callback)
    {
        return evented.On("dragend", callback);
    }

    public static Task OnDragStart(this Evented evented, Func<MouseEvent, Task> callback)
    {
        return evented.On("dragstart", callback);
    }

    public static Task OnMoveEnd(this Evented evented, Func<MouseEvent, Task> callback)
    {
        return evented.On("moveend", callback);
    }
}