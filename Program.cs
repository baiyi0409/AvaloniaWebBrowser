using Avalonia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebBrowserDemo.Extensions;
using Xilium.CefGlue;
using Xilium.CefGlue.Common;
using Xilium.CefGlue.Common.Shared;

namespace WebBrowserDemo;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main(string[] args)
    {
        var cachePath = Path.Combine(Path.GetTempPath(), "CefGlue_" + Guid.NewGuid().ToString().Replace("-", null));
        AppDomain.CurrentDomain.ProcessExit += delegate { Cleanup(cachePath); };
        CefSettings settings = new CefSettings();
        settings.CachePath = cachePath;
#if WINDOWLESS 

#else
        //IsOSREnabled = settings.WindowlessRenderingEnabled;
        settings.WindowlessRenderingEnabled = false;
#endif
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .With(new Win32PlatformOptions())
            .AfterSetup(_ => 
                CefRuntimeLoader.Initialize(
                settings,
                flags: new Dictionary<string, string> 
                {
                    {"--ignore-urlfetcher-cert-requests", "1" },
                    {"--ignore-certificate-errors", "1" },
                    {"--disable-web-security", "1" },
                    {"--no-sandbox","1"},
                    {"disable-keyring-access","1" },
                    {"disable-gpu", "1" },
                    {"disable-gpu-compositing", "1" },
                    {"enable-begin-frame-scheduling", "1" },
                    {"disable-gpu-vsync", "1" },
                    {"--disable-gpu-sandbox","1" },
                }.ToArray(),
                customSchemes: new[] {
                    new CustomScheme()
                    {
                        SchemeName = "test",
                        SchemeHandlerFactory = new CustomSchemeHandler()
                    }
                }))
            .StartWithClassicDesktopLifetime(args);
        
        return 0;
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    
    private static void Cleanup(string cachePath)
    {
        CefRuntime.Shutdown(); // must shutdown cef to free cache files (so that cleanup is able to delete files)

        try {
            var dirInfo = new DirectoryInfo(cachePath);
            if (dirInfo.Exists) {
                dirInfo.Delete(true);
            }
        } catch (UnauthorizedAccessException) {
            // ignore
        } catch (IOException) {
            // ignore
        }
    }
}