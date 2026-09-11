using Microsoft.Extensions.Logging;
using func_altinn_p360_sync_prod.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace func_altinn_p360_sync_prod.Services;

public class SkarvFunction
{
    private readonly ILogger<SkarvFunction> _logger;
    private readonly SkarvService _skarvService;

    public SkarvFunction(ILogger<SkarvFunction> logger, SkarvService skarvService)
    {
        _logger = logger;
        _skarvService = skarvService;
    }

    [Function("GetSkarvPerson")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("Fetching Skarv person from APIM");

        var results = await _skarvService.GetPersonsByFirstname();

        if (results == null || !results.Any())
        {
            return new OkObjectResult(new {
                Message = "Success",
                RecordsProcessed = 0,
                Details = "The queue is currently empty."
            });
        }

        return new OkObjectResult(results);
    }
}