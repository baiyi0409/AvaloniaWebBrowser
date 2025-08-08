using Xilium.CefGlue;
using Xilium.CefGlue.Common.Handlers;

namespace WebBrowserDemo.Extensions;

public class WebLifeSpanHandler : LifeSpanHandler
{
    protected override bool DoClose(CefBrowser browser)
    {
        //return base.DoClose(browser);
        return false;
    }

    protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName,
        CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures,
        CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo,
        ref bool noJavascriptAccess)
    {
        //return base.OnBeforePopup(browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, popupFeatures, windowInfo, ref client, settings, ref extraInfo, ref noJavascriptAccess);.LoadUrl(targetUrl);
        frame.LoadUrl(targetUrl);
        return true;
    }
}