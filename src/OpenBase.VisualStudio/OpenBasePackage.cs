using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Shell;
using OpenBase.VisualStudio.Services;
using OpenBase.VisualStudio.ToolWindows;
using Task = System.Threading.Tasks.Task;

namespace OpenBase.VisualStudio;

[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[Guid("6A7B8C9D-E0F1-4234-B567-C8D9E0F1A2B3")]
[ProvideMenuResource("Menus.ctmenu", 1)]
[ProvideToolWindow(typeof(SqlRunnerWindow))]
[ProvideToolWindow(typeof(HttpRunnerWindow))]
[ProvideToolWindow(typeof(ErDiagramWindow))]
[ProvideToolWindow(typeof(MigrationRunnerWindow))]
[ProvideToolWindow(typeof(MonitorWindow))]
public sealed class OpenBasePackage : AsyncPackage
{
    public static IServiceProvider ServiceProvider { get; private set; }

    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
        await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        await Commands.OpenBaseCommands.InitializeAsync(this);

        var logger = ServiceProvider.GetService<ILoggingService>();
        await logger.LogAsync("OpenBase extension initialized successfully.");
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IServiceProvider>(this);
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<ICliService, CliService>();
        services.AddSingleton<IWebViewService, WebViewService>();
        services.AddSingleton<ISqlService, SqlService>();
        services.AddSingleton<IHttpService, HttpService>();
        services.AddSingleton<IErService, ErService>();
    }
}
