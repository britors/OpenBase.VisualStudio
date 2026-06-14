using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBase.VisualStudio.Services;

public interface ICliService
{
    Task<CliResult> ExecuteAsync(string arguments, CancellationToken cancellationToken = default);
    Task<bool> IsCliInstalledAsync();
}

public class CliResult
{
    public int ExitCode { get; set; }
    public string StandardOutput { get; set; }
    public string StandardError { get; set; }
    public bool Success => ExitCode == 0;
}
