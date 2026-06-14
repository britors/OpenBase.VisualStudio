using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using OpenBase.VisualStudio.ToolWindows;
using Task = System.Threading.Tasks.Task;

namespace OpenBase.VisualStudio.Commands;

internal sealed class OpenBaseCommands
{
    public static readonly Guid CommandSet = new Guid("7B8C9D0E-F1A2-4345-C678-D9E0F1A2B3C4");

    public static async Task InitializeAsync(AsyncPackage package)
    {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

        var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as IMenuCommandService;
        if (commandService != null)
        {
            RegisterCommand(commandService, 0x0100, () => ShowToolWindow<SqlRunnerWindow>(package));
            RegisterCommand(commandService, 0x0200, () => ShowToolWindow<HttpRunnerWindow>(package));
            RegisterCommand(commandService, 0x0300, () => ShowToolWindow<ErDiagramWindow>(package));
            RegisterCommand(commandService, 0x0400, () => ShowToolWindow<MigrationRunnerWindow>(package));
            RegisterCommand(commandService, 0x0500, () => ShowToolWindow<MonitorWindow>(package));
        }
    }

    private static void RegisterCommand(IMenuCommandService service, int id, Action action)
    {
        var commandId = new CommandID(CommandSet, id);
        var menuItem = new MenuCommand((s, e) => action(), commandId);
        service.AddCommand(menuItem);
    }

    private static void ShowToolWindow<T>(AsyncPackage package) where T : ToolWindowPane
    {
        _ = package.JoinableTaskFactory.RunAsync(async () =>
        {
            var window = await package.ShowToolWindowAsync(typeof(T), 0, true, package.DisposalToken);
            if (window == null || window.Frame == null)
            {
                throw new NotSupportedException("Cannot create tool window");
            }
            
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        });
    }
}
