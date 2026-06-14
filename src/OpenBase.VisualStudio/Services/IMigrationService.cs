using System.Threading;
using System.Threading.Tasks;

namespace OpenBase.VisualStudio.Services;

public interface IMigrationService
{
    Task<CliResult> MigrateUpAsync(string connectionId, CancellationToken cancellationToken = default);
    Task<CliResult> MigrateDownAsync(string connectionId, CancellationToken cancellationToken = default);
    Task<CliResult> DryRunAsync(string connectionId, CancellationToken cancellationToken = default);
}

public class MigrationService(ICliService cliService) : IMigrationService
{
    public async Task<CliResult> MigrateUpAsync(string connectionId, CancellationToken cancellationToken = default) 
        => await cliService.ExecuteAsync($"migrate up --connection \"{connectionId}\"", cancellationToken);

    public async Task<CliResult> MigrateDownAsync(string connectionId, CancellationToken cancellationToken = default) 
        => await cliService.ExecuteAsync($"migrate down --connection \"{connectionId}\"", cancellationToken);

    public async Task<CliResult> DryRunAsync(string connectionId, CancellationToken cancellationToken = default) 
        => await cliService.ExecuteAsync($"migrate dry-run --connection \"{connectionId}\"", cancellationToken);
}
