using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBase.VisualStudio.Services;

public interface ISqlService
{
    Task<DataTable> ExecuteQueryAsync(string connectionId, string sql, CancellationToken cancellationToken = default);
    Task<List<string>> GetConnectionsAsync();
}

public class SqlService(ICliService cliService) : ISqlService
{
    public async Task<DataTable> ExecuteQueryAsync(string connectionId, string sql, CancellationToken cancellationToken = default)
    {
        // O CLI openbase deve suportar o comando query que retorna JSON
        var escapedSql = sql.Replace("\"", "\\\"");
        var result = await cliService.ExecuteAsync($"query --connection \"{connectionId}\" --sql \"{escapedSql}\" --format json", cancellationToken);

        if (!result.Success)
        {
            throw new Exception(result.StandardError);
        }

        return JsonToDataTable(result.StandardOutput);
    }

    public async Task<List<string>> GetConnectionsAsync()
    {
        var result = await cliService.ExecuteAsync("connection list --format json");
        if (!result.Success) return new List<string>();

        try
        {
            return JsonSerializer.Deserialize<List<string>>(result.StandardOutput);
        }
        catch
        {
            return new List<string>();
        }
    }

    private DataTable JsonToDataTable(string json)
    {
        var dt = new DataTable();
        if (string.IsNullOrWhiteSpace(json)) return dt;

        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Array) return dt;

            bool columnsCreated = false;

            foreach (var row in doc.RootElement.EnumerateArray())
            {
                if (!columnsCreated)
                {
                    foreach (var prop in row.EnumerateObject())
                    {
                        dt.Columns.Add(prop.Name);
                    }
                    columnsCreated = true;
                }

                var dr = dt.NewRow();
                foreach (var prop in row.EnumerateObject())
                {
                    dr[prop.Name] = prop.Value.ToString();
                }
                dt.Rows.Add(dr);
            }
        }
        catch
        {
            // Log error
        }

        return dt;
    }
}
