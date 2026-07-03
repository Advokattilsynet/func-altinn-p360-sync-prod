using Microsoft.Extensions.Logging;
using func_altinn_p360_sync_prod.Models;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net;

namespace func_altinn_p360_sync_prod.Services;

public class AltinnService
{
    private readonly ILogger<AltinnService> _logger;
    private readonly HttpClient _httpClient;
    private const string _apimSubcriptionKeyHeader = "Ocp-Apim-Subscription-Key";
    private string _apimSubcriptionKey;

    public AltinnService(HttpClient httpClient, ILogger<AltinnService> logger)
    {
        _logger = logger;
        _httpClient = httpClient;
        _apimSubcriptionKey = Environment.GetEnvironmentVariable("APIM_SUBSCRIPTION_KEY") ?? ""; // should probably be removed in favor of Azure Secrets
    }

    public async Task<List<AltinnSoknadsskjema>?> GetInstances(CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching Altinn instances from APIM");

        var request = new HttpRequestMessage(HttpMethod.Get, "");
        request.Headers.TryAddWithoutValidation(_apimSubcriptionKeyHeader, _apimSubcriptionKey);

        var response = await _httpClient.SendAsync(request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            _logger.LogError("APIM Call Failed. Status: {StatusCode}, Reason: {Reason}, Content: {Content}", 
                (int)response.StatusCode, response.ReasonPhrase, errorBody);
            
            throw new HttpRequestException($"APIM returned {(int)response.StatusCode} ({response.ReasonPhrase}). Details: {errorBody}");
        }

        var json = await response.Content.ReadAsStringAsync(ct);

        var result = JsonConvert.DeserializeObject<AltinnInstances>(json);
        
        if (result?.instances != null) {
            _logger.LogInformation("Fetched {Count} instances from Altinn", result.instances.Count);
            return result.instances;
        }

        return null;
    }
}