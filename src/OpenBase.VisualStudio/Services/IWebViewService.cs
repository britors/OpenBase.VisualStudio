using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace OpenBase.VisualStudio.Services;

public interface IWebViewService
{
    Task InitializeAsync(WebView2 webView, string folderName);
    Task ExecuteScriptAsync(WebView2 webView, string script);
    void RegisterCommandHandler(WebView2 webView, Action<string> handler);
}

public class WebViewService(ILoggingService loggingService) : IWebViewService
{
    public async Task InitializeAsync(WebView2 webView, string folderName)
    {
        try
        {
            var userDataFolder = Path.Combine(Path.GetTempPath(), "OpenBase.WebView2");
            var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
            
            await webView.EnsureCoreWebView2Async(env);

            // Mapear pasta local para um domínio virtual para evitar problemas de CORS
            string baseDir = Path.GetDirectoryName(typeof(OpenBasePackage).Assembly.Location);
            string contentsDir = Path.Combine(baseDir, "Resources", "Web", folderName);
            
            if (Directory.Exists(contentsDir))
            {
                webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    "openbase.local", contentsDir, CoreWebView2HostResourceAccessKind.Allow);
                
                webView.Source = new Uri("https://openbase.local/index.html");
            }
            else
            {
                await loggingService.LogErrorAsync($"WebView assets directory not found: {contentsDir}");
            }
        }
        catch (Exception ex)
        {
            await loggingService.LogErrorAsync("Failed to initialize WebView2", ex);
        }
    }

    public async Task ExecuteScriptAsync(WebView2 webView, string script)
    {
        if (webView?.CoreWebView2 != null)
        {
            await webView.CoreWebView2.ExecuteScriptAsync(script);
        }
    }

    public void RegisterCommandHandler(WebView2 webView, Action<string> handler)
    {
        if (webView?.CoreWebView2 != null)
        {
            webView.CoreWebView2.WebMessageReceived += (s, e) =>
            {
                handler(e.TryGetWebMessageAsString());
            };
        }
    }
}
