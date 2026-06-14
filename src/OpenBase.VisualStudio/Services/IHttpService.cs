using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBase.VisualStudio.Services;

public interface IHttpService
{
    Task<HttpResponseModel> SendRequestAsync(HttpRequestModel request, CancellationToken cancellationToken = default);
}

public class HttpService(ICliService cliService) : IHttpService
{
    public async Task<HttpResponseModel> SendRequestAsync(HttpRequestModel request, CancellationToken cancellationToken = default)
    {
        // Criamos um payload temporário para passar os dados complexos ao CLI
        var requestJson = JsonSerializer.Serialize(request);

        // O CLI openbase deve suportar o comando http que aceita um payload JSON ou argumentos
        // Aqui simulamos a passagem via argumentos por simplicidade, mas o ideal seria um arquivo temporário
        var args = $"http --method {request.Method} --url \"{request.Url}\"";
        if (!string.IsNullOrEmpty(request.Body))
        {
            var escapedBody = request.Body.Replace("\"", "\\\"");
            args += $" --body \"{escapedBody}\"";
        }

        var result = await cliService.ExecuteAsync(args, cancellationToken);

        if (!result.Success && string.IsNullOrEmpty(result.StandardOutput))
        {
            throw new Exception(result.StandardError);
        }

        try 
        {
            return JsonSerializer.Deserialize<HttpResponseModel>(result.StandardOutput, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch
        {
            return new HttpResponseModel 
            { 
                StatusCode = result.ExitCode == 0 ? 200 : 500,
                Body = result.StandardOutput + result.StandardError
            };
        }
    }
}

public class HttpRequestModel
{
    public string Method { get; set; } = "GET";
    public string Url { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    public string Body { get; set; }
}

public class HttpResponseModel
{
    public int StatusCode { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    public long ElapsedMilliseconds { get; set; }
}
