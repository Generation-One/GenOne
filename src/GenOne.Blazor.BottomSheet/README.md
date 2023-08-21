# GenOne.Blazor.BottomSheet

[![nuget](https://img.shields.io/nuget/v/GenOne.Blazor.BottomSheet?style=flat-square)](https://www.nuget.org/packages/GenOne.Blazor.BottomSheet)
[![downloads](https://img.shields.io/nuget/dt/GenOne.Blazor.BottomSheet?style=flat-square)](https://www.nuget.org/packages/GenOne.Blazor.BottomSheet)
[![lisence](https://img.shields.io/badge/lisence-MIT-green?style=flat-square)](https://github.com/Generation-One/GenOne.Blazor.BottomSheet/blob/master/LICENSE)

GenOne.Blazor.BottomSheet is draggable bottom sheet for blazor apps and blazor hybrid apps

## Getting started

Register services
```c#
  builder.Services.AddBottomSheet();
```
Setup content, use `data-bs-header` attribute for footer - always possible to drag, `data-bs-content` - for content, if content is scrollable add `<div data-bs-watch></div>` to the beggining of content it observe intersections and it block drag when scrolling, when the content scrolled to the top its possible to drag bottom sheet down, `data-bs-footer` - same as header - always draggable
```html
    <table class="table">
        <thead data-bs-header>
            <tr>
                <th>Date</th>
            </tr>
        </thead>
        <tbody data-bs-content>
            <div data-bs-watch></div>
            <tr>
                <td>Date</td>
            </tr>
            <tr data-bs-footer>
                <td>Footer</td>
            </tr>
        </tbody>
    </table>
```
Use component, [BlazorHybrid example](https://github.com/Generation-One/GenOne.Blazor.BottomSheet/tree/master/demo/BlazorHybrid)
```c#
Times closed: @_closedCount;

<button @onclick="OpenSheet">Open Sheet</button>

<BottomSheetBlazorJs @ref="_sheet" Sensitivity="5" Stops="new[] { 95 }" OnClosed="() => _closedCount++">
    <FetchData></FetchData>
</BottomSheetBlazorJs>


@code
{
    private BottomSheetBlazorJs? _sheet;

    private int _closedCount;

    public async Task OpenSheet()
    {
        if (_sheet is null)
            throw new ArgumentNullException();

        await _sheet.Open();
    }
}
```
