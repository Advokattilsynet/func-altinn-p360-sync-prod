using Microsoft.Extensions.Logging;
using func_altinn_p360_sync_prod.Models;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Linq;

namespace func_altinn_p360_sync_prod.Services;

public class AltinnService
{
    private readonly ILogger<AltinnService> _logger;
    private readonly HttpClient _httpClient;
    private const string _apimSubcriptionKeyHeader = "Ocp-Apim-Subscription-Key";
    private string _apimSubcriptionKey;

    public AltinnService(HttpClient httpClient, ILogger<AltinnService> logger, IOptions<ApimSettings> settings)
    {
        _logger = logger;
        _httpClient = httpClient;
        _apimSubcriptionKey = settings.Value.SubscriptionKey;
    }

    public async Task<List<AltinnSoknadsskjema>?> GetInstances(CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching Altinn instances from APIM");

        var appId = "tra/soknad-om-advokatbevilling";
        var todaysDate = DateTime.Now.ToString("yyyy-MM-dd");
        var request = new HttpRequestMessage(HttpMethod.Get, $"?appId={appId}&process.isComplete=true&process.ended=gt:{todaysDate}");
        request.Headers.TryAddWithoutValidation(_apimSubcriptionKeyHeader, _apimSubcriptionKey);

        var response = await _httpClient.SendAsync(request, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);

            _logger.LogError($"APIM Call Failed. Status: {(int)response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {errorBody}");

            throw new HttpRequestException($"APIM returned {(int)response.StatusCode} ({response.ReasonPhrase}). Details: {errorBody}");
        }

        var json = await response.Content.ReadAsStringAsync(ct);

        var result = JsonConvert.DeserializeObject<AltinnInstances>(json);

        if (result?.instances != null) {
            _logger.LogInformation($"Fetched {result.instances.Count} instances from Altinn");
            return result.instances;
        }

        return null;
    }

    public async Task<InstanceDataResult?> GetInstanceData(string instanceId, string dataId, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{instanceId}/data/{dataId}");
        request.Headers.TryAddWithoutValidation(_apimSubcriptionKeyHeader, _apimSubcriptionKey);

        var response = await _httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode) return null;

        var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
        var result = new InstanceDataResult { ContentType = contentType };

        if (contentType == "application/pdf")
        {
            result.FileBytes = await response.Content.ReadAsByteArrayAsync(ct); // handle PDF by reading as byte array
            result.Filename = response.Content.Headers.ContentDisposition?.FileName ?? $"{dataId}.pdf";
        }
        else if (contentType.Contains("xml"))
        {
            var xml = await response.Content.ReadAsStringAsync(ct);
            var serializer = new XmlSerializer(typeof(AT_forstegangssoker));
            using var reader = new StringReader(xml);
            result.Model = (AT_forstegangssoker?)serializer.Deserialize(reader);
        }
        else if (contentType.Contains("json")) // json is not returned as of 07.2026, but is added just in case of future addition
        {
            var json = await response.Content.ReadAsStringAsync(ct);
            result.Model = JsonConvert.DeserializeObject<AT_forstegangssoker>(json);
        }

        return result;
    }

    public async Task<
        List<(
            AltinnSoknadsskjema Instance,
            AT_forstegangssoker? FormData,
            List<(string Filename, byte[] Bytes)> Files
        )>
    > GetInstancesWithData(CancellationToken ct = default)
    {
        var instances = await GetInstances(ct);
        if (instances == null) return [];

        var results = new List<(AltinnSoknadsskjema Instance, AT_forstegangssoker? Model, List<(string Filename, byte[] Bytes)> Files)>();

        foreach (var instance in instances)
        {
            if (instance.data == null) continue;

            AT_forstegangssoker? model = null;
            var files = new List<(string Filename, byte[] Bytes)>();

            foreach (var dataElement in instance.data)
            {
                var result = await GetInstanceData(instance.id, dataElement.id, ct);

                if (result == null)
                    continue;

                if (result.Model != null)
                {
                    model = result.Model;
                }

                if (result.FileBytes != null)
                {
                    files.Add((result.Filename ?? "unknown.pdf", result.FileBytes));
                }
            }

            if (model != null || files.Count > 0)
            {
                results.Add((instance, model, files));
            }
        }

        return results;
    }
}
