﻿using GenOne.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GenOne.Blazor.BottomSheet.JsInterop
{
    internal class BottomSheetJsInterop(IJSRuntime jsRuntime) : BaseJsInterop(jsRuntime, BottomSheetInteropPath)
    {
        private const string BottomSheetInteropName = "bottom-sheet.js";
        private const string BottomSheetInteropPath = $"{InteropConfig.BaseJsFolder}{BottomSheetInteropName}";

        public async ValueTask<BottomSheetReference> InitializeBottomSheet(string sheetId, bool passive, int sensitivity, Func<Task> onClosed)
        {
            var module = await EnsureModuleImported();
            var handler = JsHandlerFactory.AsyncCallbackHandler(onClosed);
            var bsRef = await module.InvokeAsync<IJSObjectReference>("initializeBottomSheet", sheetId, passive, sensitivity, handler);
            return new BottomSheetReference(bsRef);
        }

        public async ValueTask AddBottomOffset(string sheetId, string offset)
        {
            var module = await EnsureModuleImported();
            await module.InvokeVoidAsync("addBottomOffset", sheetId, offset);
        }
    }
}
