using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace OpenBase.VisualStudio.Infrastructure;

public class OptionsPage : DialogPage
{
    [Category("General")]
    [DisplayName("CLI Path")]
    [Description("Full path to the openbase CLI executable.")]
    public string CliPath { get; set; } = "openbase";

    [Category("General")]
    [DisplayName("Log Level")]
    [Description("Log level for the extension.")]
    public LogLevel LogLevel { get; set; } = LogLevel.Info;
}

public enum LogLevel
{
    Verbose,
    Info,
    Error
}
