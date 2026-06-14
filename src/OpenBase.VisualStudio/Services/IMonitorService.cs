using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBase.VisualStudio.Services;

public interface IMonitorService
{
    Task StartMonitoringAsync(Action<string> onLogReceived, CancellationToken cancellationToken = default);
}

public class MonitorService(ICliService cliService) : IMonitorService
{
    public async Task StartMonitoringAsync(Action<string> onLogReceived, CancellationToken cancellationToken = default)
    {
        await cliService.ExecuteStreamingAsync("monitor --format json", onLogReceived, cancellationToken);
    }
}
