using Microsoft.Extensions.Logging;
using func_altinn_p360_sync_prod.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace func_altinn_p360_sync_prod.Services;

public class SyncFunction
{
    private readonly ILogger<SyncFunction> _logger;
    private readonly AltinnService _altinnService;

    public SyncFunction(ILogger<SyncFunction> logger, AltinnService altinnService)
    {
        _logger = logger;
        _altinnService = altinnService;
    }

    [Function("SyncAltinnToP360")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("Fetching Altinn instances from APIM");

        var altinnData = await _altinnService.GetInstancesWithData();

        if (altinnData == null || !altinnData.Any())
        {
            return new OkObjectResult(new {
                Message = "Success",
                RecordsProcessed = 0,
                Details = "The queue is currently empty."
            });
        }

        var output = altinnData.Select(r => new
        {
            InstanceId = r.Instance.id,
            PartyId = r.Instance.instanceOwner?.partyId,
            FormData = r.FormData
        }).ToList();

        return new OkObjectResult(output);
    }
}