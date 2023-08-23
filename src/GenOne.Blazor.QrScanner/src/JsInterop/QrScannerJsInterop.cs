using GenOne.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GenOne.Blazor.QrScanner
{
    internal class QrScannerJsInterop : BaseJsInterop
    {
        private const string QrScannerInteropName = "qr-scanner-interop.js";
        private const string QrScannerInteropPath = $"{InteropConfig.BaseJsFolder}{QrScannerInteropName}";

        public QrScannerJsInterop(IJSRuntime jsRuntime) : base(jsRuntime, QrScannerInteropPath)
        { }

        public async ValueTask<IJSObjectReference> CreateQrScannerInstance(ElementReference videoElementReference, Func<string, Task> onSuccessScanning)
        {
            var module = await EnsureModuleImported();
            var handler = JsHandlerFactory.AsyncCallbackHandler(onSuccessScanning);
            return await module.InvokeAsync<IJSObjectReference>("createQrScanner", videoElementReference, handler);
        }

        public async ValueTask StartScanning(IJSObjectReference qrScannerReference)
        {
            var module = await EnsureModuleImported();
            await module.InvokeVoidAsync("startScanning", qrScannerReference);
        }

        public async ValueTask StopScanning(IJSObjectReference qrScannerReference)
        {
            var module = await EnsureModuleImported();
            await module.InvokeVoidAsync("stopScanning", qrScannerReference);
        }

        public async ValueTask DisposeQrScanner(IJSObjectReference qrScannerReference)
        {
            var module = await EnsureModuleImported();
            await module.InvokeVoidAsync("disposeQrScanner", qrScannerReference);
        }
    }
}
