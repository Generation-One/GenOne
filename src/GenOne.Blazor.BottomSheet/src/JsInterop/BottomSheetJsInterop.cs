using GenOne.JsInterop;
using Microsoft.JSInterop;

namespace GenOne.Blazor.BottomSheet.JsInterop
{
    internal class BottomSheetJsInterop : BaseJsInterop
    {
        private const string BottomSheetInteropName = "bottom-sheet.js";
        private const string BottomSheetInteropPath = $"{InteropConfig.BaseJsFolder}{BottomSheetInteropName}";

        public BottomSheetJsInterop(IJSRuntime jsRuntime) : base(jsRuntime, BottomSheetInteropPath)
        {
        }

        internal async ValueTask<IJSObjectReference> InitializeBottomSheet(int[] stops, bool passive, int sensitivity, Func<Task> onClosed)
        {
            var module = await EnsureModuleImported();
            var handler = JsHandlerFactory.AsyncCallbackHandler(onClosed);
            return await module.InvokeAsync<IJSObjectReference>("initializeBottomSheet", stops, passive, sensitivity, handler);
        }

        internal async ValueTask OpenBottomSheet(IJSObjectReference bottomSheetReference)
        {
            var module = await EnsureModuleImported();
            await module.InvokeVoidAsync("openBottomSheet", bottomSheetReference);
        }

        internal async ValueTask CloseBottomSheet(IJSObjectReference bottomSheetReference)
        {
            var module = await EnsureModuleImported();
            await module.InvokeVoidAsync("closeBottomSheet", bottomSheetReference);
        }
    }
}
