using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using OpenBase.VisualStudio.Services;

namespace OpenBase.VisualStudio.UI;

public partial class MonitorControl : UserControl
{
    private readonly IMonitorService _monitorService;
    private CancellationTokenSource _cts;

    public MonitorControl()
    {
        InitializeComponent();
        _monitorService = OpenBasePackage.ServiceProvider.GetService<IMonitorService>();
        _ = StartMonitoringAsync();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD001:Avoid legacy threading APIs", Justification = "Dispatcher.Invoke is required for WPF UI updates from background threads")]
    private async System.Threading.Tasks.Task StartMonitoringAsync()
    {
        _cts = new CancellationTokenSource();
        await _monitorService.StartMonitoringAsync(log => {
            Dispatcher.Invoke(() => {
                LogListBox.Items.Add(log);
                LogListBox.ScrollIntoView(LogListBox.Items[LogListBox.Items.Count - 1]);
            });
        }, _cts.Token);
    }
}
