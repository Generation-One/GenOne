# GenOne.Blazor.QrScanner

### Android workaround for MAUI
On android you can face an issue when permission for camera denied for js code. Need to grant permissions for web view

Add `BlazorWebViewInitialized` handler for `BlazorWebView`, and set web chrome client to `WebViewProxy`

```c#
        private void WebView_OnBlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {
#if ANDROID
            e.WebView.SetWebChromeClient(new WebViewProxy(e.WebView.WebChromeClient));
#endif
        }
```

WebViewProxy:
```c#
public class WebViewProxy : WebChromeClient
{
    private WebChromeClient webChromeClient;

    public override void OnPermissionRequest(PermissionRequest request)
    {
        request.Grant(request.GetResources());
    }

    public WebViewProxy(WebChromeClient webChromeClient)
        => this.webChromeClient = webChromeClient;

    public override bool OnCreateWindow(AWebView view, bool isDialog, bool isUserGesture, Message resultMsg)
        => webChromeClient.OnCreateWindow(view, isDialog, isUserGesture, resultMsg);

    public override bool OnShowFileChooser(AWebView webView, IValueCallback filePathCallback, FileChooserParams fileChooserParams)
        => webChromeClient.OnShowFileChooser(webView, filePathCallback, fileChooserParams);
}
```

