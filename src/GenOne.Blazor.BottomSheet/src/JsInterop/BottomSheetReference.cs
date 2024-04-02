using Microsoft.JSInterop;

namespace GenOne.Blazor.BottomSheet.JsInterop;

internal class BottomSheetReference(IJSObjectReference bottomSheetReference) : IAsyncDisposable
{
    public ValueTask Open(int[] stops) 
        => bottomSheetReference.InvokeVoidAsync("open", stops);

    public ValueTask Close() 
        => bottomSheetReference.InvokeVoidAsync("close");

    public ValueTask DisposeAsync() 
        => bottomSheetReference.DisposeAsync();
}