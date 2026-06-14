using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBase.VisualStudio.Services;

public interface IErService
{
    Task<string> GenerateMermaidAsync(string connectionId, CancellationToken cancellationToken = default);
}

public class ErService(ICliService cliService) : IErService
{
    public async Task<string> GenerateMermaidAsync(string connectionId, CancellationToken cancellationToken = default)
    {
        // O CLI openbase deve suportar o comando er que retorna código Mermaid
        var result = await cliService.ExecuteAsync($"er --connection \"{connectionId}\" --format mermaid", cancellationToken);

        if (!result.Success)
        {
            throw new Exception(result.StandardError);
        }

        return result.StandardOutput;
    }
}
