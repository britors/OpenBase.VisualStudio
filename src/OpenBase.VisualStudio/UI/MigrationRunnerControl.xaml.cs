using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using OpenBase.VisualStudio.Services;

namespace OpenBase.VisualStudio.UI;

public partial class MigrationRunnerControl : UserControl
{
    private readonly IMigrationService _migrationService;
    private readonly IScaffoldService _scaffoldService;
    private readonly ISqlService _sqlService;

    public MigrationRunnerControl()
    {
        InitializeComponent();
        _migrationService = OpenBasePackage.ServiceProvider.GetService<IMigrationService>();
        _scaffoldService = OpenBasePackage.ServiceProvider.GetService<IScaffoldService>();
        _sqlService = OpenBasePackage.ServiceProvider.GetService<ISqlService>();

        _ = LoadConnectionsAsync();
    }

    private async System.Threading.Tasks.Task LoadConnectionsAsync()
    {
        if (_sqlService == null) return;
        var connections = await _sqlService.GetConnectionsAsync();
        ConnectionCombo.ItemsSource = connections;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "WPF event handler")]
    private async void MigrateUp_Click(object sender, RoutedEventArgs e) => await ExecuteMigrationAsync(c => _migrationService.MigrateUpAsync(c));

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "WPF event handler")]
    private async void MigrateDown_Click(object sender, RoutedEventArgs e) => await ExecuteMigrationAsync(c => _migrationService.MigrateDownAsync(c));

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "WPF event handler")]
    private async void DryRun_Click(object sender, RoutedEventArgs e) => await ExecuteMigrationAsync(c => _migrationService.DryRunAsync(c));

    private async System.Threading.Tasks.Task ExecuteMigrationAsync(Func<string, System.Threading.Tasks.Task<CliResult>> action)
    {
        var connection = ConnectionCombo.Text;
        if (string.IsNullOrWhiteSpace(connection) || connection == "Select Connection...") return;

        try {
            var result = await action(connection);
            MessageBox.Show(result.Success ? "Success" : "Error: " + result.StandardError);
        }
        catch (System.Exception ex) {
            MessageBox.Show(ex.Message);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "WPF event handler")]
    private async void Scaffold_Click(object sender, RoutedEventArgs e)
    {
        var connection = ConnectionCombo.Text;
        var output = OutputDirText.Text;
        if (string.IsNullOrWhiteSpace(connection) || connection == "Select Connection...") return;

        try {
            var result = await _scaffoldService.ScaffoldAsync(connection, output);
            MessageBox.Show(result.Success ? "Scaffold successful" : "Error: " + result.StandardError);
        }
        catch (System.Exception ex) {
            MessageBox.Show(ex.Message);
        }
    }
}
