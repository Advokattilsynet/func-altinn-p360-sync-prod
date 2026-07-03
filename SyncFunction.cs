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

        var instances = await _altinnService.GetInstances();

        if (instances == null)
        {
            return new BadRequestObjectResult(new {
                Error = "Service Connectivity Issue",
                Details = "The Altinn service returned null. This usually indicates a timeout or authentication failure."
            });
        }
        
        if (!instances.Any())
        {
            return new OkObjectResult(new {
                Message = "Success",
                RecordsProcessed = 0,
                Details = "The queue is currently empty."
            });
        }

        foreach (var instance in instances)
        {
            _logger.LogInformation("Processing Instance: {InstanceId}", instance.ToString());
        }
    
        return new OkObjectResult(instances);
    }
}