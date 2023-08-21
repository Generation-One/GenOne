using Microsoft.JSInterop;

namespace GenOne.JsInterop
{
    public static class JsHandlerFactory
    {
        public static DotNetObjectReference<JsEventHandler> VoidCallbackHandler(Action callback)
        {
            return new JsEventHandler(callback).ObjRef;
        }

        public static DotNetObjectReference<JsEventHandler<T>> CallbackHandler<T>(Action<T> callback)
        {
            return new JsEventHandler<T>(callback).ObjRef;
        }

        public static DotNetObjectReference<JsAsyncEventHandler> AsyncCallbackHandler(Func<Task> callback)
        {
            return new JsAsyncEventHandler(callback).ObjRef;
        }

        public static DotNetObjectReference<JsAsyncEventHandler<T>> AsyncCallbackHandler<T>(Func<T, Task> callback)
        {
            return new JsAsyncEventHandler<T>(callback).ObjRef;
        }

        public class JsEventHandler
        {
            private readonly Action _callback;
            public DotNetObjectReference<JsEventHandler> ObjRef { get; }

            public JsEventHandler(Action callback)
            {
                _callback = callback;
                ObjRef = DotNetObjectReference.Create(this);
            }

            [JSInvokable]
            public void Callback() => _callback();
        }

        public class JsEventHandler<T>
        {
            private readonly Action<T> _callback;
            public DotNetObjectReference<JsEventHandler<T>> ObjRef { get; }

            public JsEventHandler(Action<T> callback)
            {
                _callback = callback;
                ObjRef = DotNetObjectReference.Create(this);
            }

            [JSInvokable]
            public void Callback(T parameter) => _callback(parameter);
        }

        public class JsAsyncEventHandler
        {
            private readonly Func<Task> _callback;
            public DotNetObjectReference<JsAsyncEventHandler> ObjRef { get; }

            public JsAsyncEventHandler(Func<Task> callback)
            {
                _callback = callback;
                ObjRef = DotNetObjectReference.Create(this);
            }

            [JSInvokable]
            public Task Callback() => _callback();
        }

        public class JsAsyncEventHandler<T>
        {
            private readonly Func<T, Task> _callback;
            public DotNetObjectReference<JsAsyncEventHandler<T>> ObjRef { get; }

            public JsAsyncEventHandler(Func<T, Task> callback)
            {
                _callback = callback;
                ObjRef = DotNetObjectReference.Create(this);
            }

            [JSInvokable]
            public Task Callback(T data) => _callback(data);
        }
    }
}
