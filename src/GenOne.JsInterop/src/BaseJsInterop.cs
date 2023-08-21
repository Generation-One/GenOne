using Microsoft.JSInterop;

namespace GenOne.JsInterop
{
    public class BaseJsInterop : IAsyncDisposable
    {
        protected readonly Lazy<Task<IJSObjectReference>> _moduleTask;
        protected const string Import = "import";

        public BaseJsInterop(IJSRuntime jsRuntime, string jsFilePath)
        {
            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               Import, jsFilePath).AsTask());
        }

        protected Task<IJSObjectReference> EnsureModuleImported()
        {
            return _moduleTask.Value;
        }

        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
