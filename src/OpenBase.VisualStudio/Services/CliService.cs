using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBase.VisualStudio.Services;

public class CliService(ILoggingService loggingService) : ICliService
{
    public async Task<CliResult> ExecuteAsync(string arguments, CancellationToken cancellationToken = default)
    {
        var result = new CliResult();
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "openbase",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (s, e) => { if (e.Data != null) outputBuilder.AppendLine(e.Data); };
            process.ErrorDataReceived += (s, e) => { if (e.Data != null) errorBuilder.AppendLine(e.Data); };

            await loggingService.LogAsync($"Executing: openbase {arguments}");

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            using (cancellationToken.Register(() => { try { process.Kill(); } catch { } }))
            {
                await Task.Run(() => process.WaitForExit(), cancellationToken);
            }

            result.ExitCode = process.ExitCode;
            result.StandardOutput = outputBuilder.ToString();
            result.StandardError = errorBuilder.ToString();

            if (!result.Success)
            {
                await loggingService.LogErrorAsync($"Command failed with exit code {result.ExitCode}: {result.StandardError}");
            }
        }
        catch (Exception ex)
        {
            result.ExitCode = -1;
            result.StandardError = ex.Message;
            await loggingService.LogErrorAsync("Failed to execute openbase CLI", ex);
        }

        return result;
    }

    public async Task<bool> IsCliInstalledAsync()
    {
        var result = await ExecuteAsync("--version");
        return result.Success;
    }
}
