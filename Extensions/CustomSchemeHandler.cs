using Xilium.CefGlue;

namespace WebBrowserDemo.Extensions;

public class CustomSchemeHandler: CefSchemeHandlerFactory
{
    protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
    {
        throw new System.NotImplementedException();
    }
}