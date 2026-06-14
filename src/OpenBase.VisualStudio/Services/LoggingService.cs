using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace OpenBase.VisualStudio.Services;

public class LoggingService(IServiceProvider serviceProvider) : ILoggingService
{
    private IVsOutputWindowPane _pane;
    private static readonly Guid _paneGuid = new Guid("4A5B6C7D-8E9F-A0B1-C2D3-E4F5A6B7C8D9");

    private async Task<IVsOutputWindowPane> GetPaneAsync()
    {
        if (_pane != null) return _pane;

        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
        var outputWindow = serviceProvider.GetService(typeof(SVsOutputWindow)) as IVsOutputWindow;
        
        if (outputWindow != null)
        {
            Guid paneGuid = _paneGuid;
            outputWindow.GetPane(ref paneGuid, out _pane);
            if (_pane == null)
            {
                outputWindow.CreatePane(ref paneGuid, "OpenBase", 1, 1);
                outputWindow.GetPane(ref paneGuid, out _pane);
            }
        }
        
        return _pane;
    }

    public async Task LogAsync(string message)
    {
        var pane = await GetPaneAsync();
        if (pane != null)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            pane.OutputStringThreadSafe($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }
    }

    public async Task LogErrorAsync(string message, Exception ex = null)
    {
        var pane = await GetPaneAsync();
        if (pane != null)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var errorMessage = ex != null ? $"{message}: {ex.Message}" : message;
            pane.OutputStringThreadSafe($"[{DateTime.Now:HH:mm:ss}] ERROR: {errorMessage}{Environment.NewLine}");
        }
    }
}
