using System.Threading;
using System.Threading.Tasks;

namespace OpenBase.VisualStudio.Services;

public interface IScaffoldService
{
    Task<CliResult> ScaffoldAsync(string connectionId, string outputDir, CancellationToken cancellationToken = default);
}

public class ScaffoldService(ICliService cliService) : IScaffoldService
{
    public async Task<CliResult> ScaffoldAsync(string connectionId, string outputDir, CancellationToken cancellationToken = default) 
        => await cliService.ExecuteAsync($"scaffold --connection \"{connectionId}\" --output \"{outputDir}\"", cancellationToken);
}
