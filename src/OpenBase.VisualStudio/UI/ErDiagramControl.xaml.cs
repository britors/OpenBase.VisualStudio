using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using OpenBase.VisualStudio.Services;

namespace OpenBase.VisualStudio.UI
{
    public partial class ErDiagramControl : UserControl
    {
        private readonly IErService _erService;
        private readonly ISqlService _sqlService;
        private readonly IWebViewService _webViewService;

        public ErDiagramControl()
        {
            InitializeComponent();
            _erService = OpenBasePackage.ServiceProvider.GetService<IErService>();
            _sqlService = OpenBasePackage.ServiceProvider.GetService<ISqlService>();
            _webViewService = OpenBasePackage.ServiceProvider.GetService<IWebViewService>();

            _ = InitializeAsync();
        }

        private async System.Threading.Tasks.Task InitializeAsync()
        {
            if (_webViewService != null)
            {
                await _webViewService.InitializeAsync(DiagramWebView, "mermaid");
            }
            await LoadConnectionsAsync();
        }

        private async System.Threading.Tasks.Task LoadConnectionsAsync()
        {
            if (_sqlService == null) return;
            var connections = await _sqlService.GetConnectionsAsync();
            ConnectionCombo.ItemsSource = connections;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Required for WPF event handler")]
        private async void GenerateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var connection = ConnectionCombo.Text;
                if (string.IsNullOrWhiteSpace(connection) || connection == "Select Connection...") return;

                GenerateButton.IsEnabled = false;
                StatusText.Text = "Generating...";

                var mermaidCode = await _erService.GenerateMermaidAsync(connection);

                // Enviar o código Mermaid para a WebView renderizar
                await DiagramWebView.CoreWebView2.ExecuteScriptAsync($"window.renderDiagram(`{mermaidCode}`)");

                StatusText.Text = "Diagram generated.";
            }
            catch (System.Exception ex)
            {
                StatusText.Text = "Error: " + ex.Message;
                System.Windows.MessageBox.Show(ex.Message, "ER Diagram Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                GenerateButton.IsEnabled = true;
            }
        }
    }
}
