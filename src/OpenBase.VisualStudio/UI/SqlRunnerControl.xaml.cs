using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using OpenBase.VisualStudio.Services;

namespace OpenBase.VisualStudio.UI
{
    public partial class SqlRunnerControl : UserControl
    {
        private readonly ISqlService _sqlService;
        private readonly IWebViewService _webViewService;

        public SqlRunnerControl()
        {
            InitializeComponent();
            _sqlService = OpenBasePackage.ServiceProvider.GetService<ISqlService>();
            _webViewService = OpenBasePackage.ServiceProvider.GetService<IWebViewService>();
            
            _ = InitializeAsync();
        }

        private async System.Threading.Tasks.Task InitializeAsync()
        {
            if (_webViewService != null)
            {
                await _webViewService.InitializeAsync(EditorWebView, "monaco");

                // Aguarda o CoreWebView2 estar pronto para enviar a mensagem de tema
                EditorWebView.CoreWebView2.NavigationCompleted += (s, e) =>
                {
                    var isDark = Microsoft.VisualStudio.PlatformUI.VSColorTheme.GetThemedColor(Microsoft.VisualStudio.PlatformUI.EnvironmentColors.ToolWindowBackgroundColorKey).GetBrightness() < 0.5;
                    _ = _webViewService.ExecuteScriptAsync(EditorWebView, $"window.setTheme('{ (isDark ? "dark" : "light") }')");
                };
            }
            await LoadConnectionsAsync();
        }

        private async System.Threading.Tasks.Task LoadConnectionsAsync()
        {
            if (_sqlService == null) return;
            var connections = await _sqlService.GetConnectionsAsync();
            ConnectionCombo.ItemsSource = connections;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Required for WPF event handler and protected by try-catch")]
        private async void ExecuteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var sql = await GetSqlFromEditorAsync();
                if (string.IsNullOrWhiteSpace(sql)) return;

                var connection = ConnectionCombo.Text;
                
                ExecuteButton.IsEnabled = false;
                StatusText.Text = "Executing...";
                ResultGrid.ItemsSource = null;

                var result = await _sqlService.ExecuteQueryAsync(connection, sql);
                ResultGrid.ItemsSource = result.DefaultView;
                StatusText.Text = $"Done. {result.Rows.Count} rows.";
            }
            catch (System.Exception ex)
            {
                StatusText.Text = "Error: " + ex.Message;
                System.Windows.MessageBox.Show(ex.Message, "SQL Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                ExecuteButton.IsEnabled = true;
            }
        }

        private async System.Threading.Tasks.Task<string> GetSqlFromEditorAsync()
        {
            // Script para pegar o valor do Monaco Editor (a ser implementado no index.html)
            var result = await EditorWebView.CoreWebView2.ExecuteScriptAsync("window.getEditorValue()");
            return System.Text.Json.JsonSerializer.Deserialize<string>(result);
        }
    }
}
