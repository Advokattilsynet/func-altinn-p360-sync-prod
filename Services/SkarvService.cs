using func_altinn_p360_sync_prod.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;

namespace func_altinn_p360_sync_prod.Services;

public class SkarvService
{
    private readonly ILogger<SkarvService> _logger;
    private readonly HttpClient _httpClient;
    private const string _apimSubcriptionKeyHeader = "Ocp-Apim-Subscription-Key";
    private string _apimSubcriptionKey;

    public SkarvService(HttpClient httpClient, ILogger<SkarvService> logger, IOptions<ApimSettings> settings)
    {
        _logger = logger;
        _httpClient = httpClient;
        _apimSubcriptionKey = settings.Value.SubscriptionKey;
    }

    public async Task<SkarvPerson?> GetPersonWithSSN(string ssn, CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching Skarv data from APIM");

        var request = new HttpRequestMessage(HttpMethod.Get, $"person?ssn={ssn}");
        request.Headers.TryAddWithoutValidation(_apimSubcriptionKeyHeader, _apimSubcriptionKey);

        var response = await _httpClient.SendAsync(request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);

            _logger.LogError($"APIM Call Failed. Status: {(int)response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {errorBody}");

            throw new HttpRequestException($"APIM returned {(int)response.StatusCode} ({response.ReasonPhrase}). Details: {errorBody}");
        }

        var json = await response.Content.ReadAsStringAsync(ct);

        var result = JsonConvert.DeserializeObject<SkarvPerson>(json);

        if (result != null) {
            _logger.LogInformation($"Fetched person from Skarv");
            return result;
        }

        return null;
    }

    public async Task<List<SkarvPerson>?> GetPersonsByFirstname(CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching Skarv data from APIM");

        var request = new HttpRequestMessage(HttpMethod.Get, $"person?firstName=");
        request.Headers.TryAddWithoutValidation(_apimSubcriptionKeyHeader, _apimSubcriptionKey);

        var response = await _httpClient.SendAsync(request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);

            _logger.LogError($"APIM Call Failed. Status: {(int)response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {errorBody}");

            throw new HttpRequestException($"APIM returned {(int)response.StatusCode} ({response.ReasonPhrase}). Details: {errorBody}");
        }

        var json = await response.Content.ReadAsStringAsync(ct);

        var result = JsonConvert.DeserializeObject<List<SkarvPerson>>(json);

        if (result != null) {
            _logger.LogInformation($"Fetched persons from Skarv");
            return result;
        }

        return null;
    }
}