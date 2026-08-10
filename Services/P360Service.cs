using func_altinn_p360_sync_prod.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace func_altinn_p360_sync_prod.Services;

public class P360Service
{
    private readonly ILogger<P360Service> _logger;
    private readonly HttpClient _httpClient;
    private const string _apimSubcriptionKeyHeader = "Ocp-Apim-Subscription-Key";
    private string _apimSubcriptionKey;
    private string _p360AuthKey;

    public P360Service(HttpClient httpClient, ILogger<P360Service> logger, IOptions<ApimSettings> settings)
    {
        _logger = logger;
        _httpClient = httpClient;
        _apimSubcriptionKey = settings.Value.SubscriptionKey;
        _p360AuthKey = settings.Value.P360AuthKey;
    }

    public async Task<P360CaseResponse?> PostCase(CancellationToken ct = default)
    {
        _logger.LogInformation("Sending new case to P360 via APIM");
   
        P360Case p360case = new P360Case
		{
			parameter = new Parameter
			{
				Title = "Søknad om advokatbevilling - AZ-test",
				Status = "B" // B = "Under behandling"
			}
		};

        var request = new HttpRequestMessage(HttpMethod.Post, $"CaseService/CreateCase?authkey={_p360AuthKey}");
        request.Headers.TryAddWithoutValidation(_apimSubcriptionKeyHeader, _apimSubcriptionKey);

        string jsonString = JsonConvert.SerializeObject(p360case);
        request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);

            _logger.LogError($"APIM P360Case Call Failed. Status: {(int)response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {errorBody}");

            throw new HttpRequestException($"APIM P360Case Call returned {(int)response.StatusCode} ({response.ReasonPhrase}). Details: {errorBody}");
        }

        var json = await response.Content.ReadAsStringAsync(ct);
        var result = JsonConvert.DeserializeObject<P360CaseResponse>(json);

        if (result != null) {
            _logger.LogInformation("Sent new case to P360");
            return result;
        }

        return null;
    }
}