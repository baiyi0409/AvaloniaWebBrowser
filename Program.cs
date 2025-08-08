using Avalonia;
using System;
using System.IO;
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
            
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .With(new Win32PlatformOptions())
            .AfterSetup(_ => CefRuntimeLoader.Initialize(new CefSettings() {
                    RootCachePath = cachePath,
#if WINDOWLESS 

#else
                    WindowlessRenderingEnabled = false
#endif
                },
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