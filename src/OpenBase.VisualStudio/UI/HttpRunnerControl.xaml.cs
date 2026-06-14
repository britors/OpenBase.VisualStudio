using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using OpenBase.VisualStudio.Services;

namespace OpenBase.VisualStudio.UI
{
    public partial class HttpRunnerControl : UserControl
    {
        private readonly IHttpService _httpService;

        public HttpRunnerControl()
        {
            InitializeComponent();
            _httpService = OpenBasePackage.ServiceProvider.GetService<IHttpService>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Required for WPF event handler")]
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var method = (MethodCombo.SelectedItem as ComboBoxItem)?.Content.ToString();
                var url = UrlText.Text;
                var body = RequestBodyText.Text;

                if (string.IsNullOrWhiteSpace(url)) return;

                SendButton.IsEnabled = false;
                StatusLabel.Text = "Sending...";
                ResponseBodyText.Text = string.Empty;

                var request = new HttpRequestModel
                {
                    Method = method,
                    Url = url,
                    Body = body
                };

                var response = await _httpService.SendRequestAsync(request);

                StatusLabel.Text = response.StatusCode.ToString();
                StatusLabel.Foreground = response.StatusCode < 400 ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red;
                TimeLabel.Text = $"{response.ElapsedMilliseconds} ms";
                ResponseBodyText.Text = response.Body;
            }
            catch (System.Exception ex)
            {
                StatusLabel.Text = "Error";
                StatusLabel.Foreground = System.Windows.Media.Brushes.Red;
                ResponseBodyText.Text = ex.Message;
            }
            finally
            {
                SendButton.IsEnabled = true;
            }
        }
    }
}
