using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using OpenBase.VisualStudio.UI;

namespace OpenBase.VisualStudio.ToolWindows;

[Guid("A1B2C3D4-E5F6-47A1-82B3-C4D5E6F7A8B9")]
public class SqlRunnerWindow : ToolWindowPane
{
    public SqlRunnerWindow() : base(null)
    {
        this.Caption = "OpenBase SQL Runner";
        this.Content = new SqlRunnerControl();
    }
}

[Guid("B2C3D4E5-F6A1-48B2-93C4-D5E6F7A8B9C0")]
public class HttpRunnerWindow : ToolWindowPane
{
    public HttpRunnerWindow() : base(null)
    {
        this.Caption = "OpenBase HTTP Runner";
        this.Content = new HttpRunnerControl();
    }
}

[Guid("C3D4E5F6-A1B2-49C3-A4D5-E6F7A8B9C0D1")]
public class ErDiagramWindow : ToolWindowPane
{
    public ErDiagramWindow() : base(null)
    {
        this.Caption = "OpenBase ER Diagram";
        this.Content = new ErDiagramControl();
    }
}

[Guid("D4E5F6A1-B2C3-4AD4-B5E6-F7A8B9C0D1E2")]
public class MigrationRunnerWindow : ToolWindowPane
{
    public MigrationRunnerWindow() : base(null)
    {
        this.Caption = "OpenBase Migration Runner";
        this.Content = new MigrationRunnerControl();
    }
}

[Guid("E5F6A1B2-C3D4-4BE5-C6F7-A8B9C0D1E2F3")]
public class MonitorWindow : ToolWindowPane
{
    public MonitorWindow() : base(null)
    {
        this.Caption = "OpenBase Monitor";
        this.Content = new MonitorControl();
    }
}
