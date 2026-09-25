using Microsoft.Extensions.Logging;
using func_altinn_p360_sync_prod.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace func_altinn_p360_sync_prod.Services;

public class SyncFunction
{
    private readonly ILogger<SyncFunction> _logger;
    private readonly AltinnService _altinnService;
    private readonly SkarvService _skarvService;
    private readonly P360Service _p360Service;

    public SyncFunction(ILogger<SyncFunction> logger, AltinnService altinnService, SkarvService skarvService, P360Service p360Service)
    {
        _logger = logger;
        _altinnService = altinnService;
        _skarvService = skarvService;
        _p360Service = p360Service;
    }

    [Function("SyncAltinnToP360")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        // ALTINN
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

        List<AltinnResult> altinnItems = altinnData.Select(r => new AltinnResult
        {
            InstanceId = r.Instance.id,
            PartyId = r.Instance.instanceOwner?.partyId,
            FormData = r.FormData,
            Files = r.Files
        }).ToList();

        foreach (var item in altinnItems) 
        {
            // SKARV
            _logger.LogInformation("Fetching each Altinn person via Skarv from APIM");
            
            var ssn = item?.FormData?.personalia?.ssn?.ToString() ?? string.Empty;
            SkarvPerson? skarvResult = await _skarvService.GetPersonWithSSN(ssn);

            if (skarvResult == null)
            {
                return new OkObjectResult(new {
                    Message = "Success",
                    RecordsProcessed = 0,
                    Details = "No person found with the provided SSN."
                });
            }

            // P360
            _logger.LogInformation("Creating new case to P360 via APIM");

            var name = skarvResult.Fornavn + skarvResult.Etternavn;
            P360CaseResponse? caseResult = await _p360Service.PostCase(name);

            _logger.LogInformation("Creating new document to P360 via APIM");
            P360DocumentResponse? documentResult = await _p360Service.PostDocument(caseResult?.CaseNumber);

            P360FileResponse? fileResult = null;
            foreach (var file in item.Files) {
                _logger.LogInformation("Creating new file to P360 via APIM");
                var fileName = file.Filename;
                var content = Convert.ToBase64String(file.Bytes);

                fileResult = await _p360Service.PostFile(fileName, documentResult?.DocumentNumber, content);
            };
        };

        return new OkObjectResult(new {
            Message = "Success",
            RecordsProcessed = 0,
            Details = "Transfer from Altinn to P360 succeeded!"
        });
    }
}