# GenOne.JsInterop helpers for interop
[![nuget](https://img.shields.io/nuget/v/GenOne.JsInterop?style=flat-square)](https://www.nuget.org/packages/GenOne.JsInterop)
[![downloads](https://img.shields.io/nuget/dt/GenOne.JsInterop?style=flat-square)](https://www.nuget.org/packages/GenOne.JsInterop)
[![lisence](https://img.shields.io/badge/lisence-MIT-green?style=flat-square)](https://github.com/Generation-One/GenOne.JsInterop/blob/master/LICENSE)

## Documentation
### JsHandlerFactory
  Provides `DotNetObjectReference` with callback to call from js

#### Example with Action:
  C#:
```c#
var action = () => Console.WriteLine("test");
var handler = JsHandlerFactory.VoidCallbackHandler(action);
return await module.InvokeAsync<IJSObjectReference>("foo", handler);
```
  Js:
```js
export function foo(handler) {
    handler.invokeMethodAsync('Callback');
}
```
Also possible to use `Action<T>`, `Func<Task>`, `Func<T, Task>`

### BaseJsInterop
Provide base class to implement interop with dynamic loading of js module.
Example from [BottomSheet implementation](https://github.com/Generation-One/GenOne.Blazor.BottomSheet/blob/master/src/GenOne.Blazor.BottomSheet/JsInterop/BottomSheetJsInterop.cs)
```c#
internal class BottomSheetJsInterop : BaseJsInterop
{
  private const string BottomSheetInteropName = "bottom-sheet.js";
  private const string BottomSheetInteropPath = $"{InteropConfig.BaseJsFolder}{BottomSheetInteropName}"; //path to js file

  public BottomSheetJsInterop(IJSRuntime jsRuntime) : base(jsRuntime, BottomSheetInteropPath)
  {
  }

  internal async ValueTask<IJSObjectReference> InitializeBottomSheet(int[] stops, bool passive, int sensitivity, Func<Task> onClosed)
  {
    var module = await EnsureModuleImported(); // always use that method to get js module
    var handler = JsHandlerFactory.AsyncCallbackHandler(onClosed);
    return await module.InvokeAsync<IJSObjectReference>("initializeBottomSheet", stops, passive, sensitivity, handler);
  }
  ...
}
```
