using Microsoft.Extensions.Logging;
using func_altinn_p360_sync_prod.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace func_altinn_p360_sync_prod.Services;

public class P360Function
{
    private readonly ILogger<P360Function> _logger;
    private readonly P360Service _p360Service;

    public P360Function(ILogger<P360Function> logger, P360Service p360Service)
    {
        _logger = logger;
        _p360Service = p360Service;
    }

    [Function("PostFullCase")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        _logger.LogInformation("Creating P360 case via APIM to Public 360");

        P360CaseResponse? caseResult = await _p360Service.PostCase("test");
        P360DocumentResponse? documentResult = await _p360Service.PostDocument(caseResult?.CaseNumber);
        P360FileResponse? fileResult = await _p360Service.PostFile("testest", documentResult?.DocumentNumber, "aGVpMTIzCg==");

        return new OkObjectResult(documentResult);
    }
}