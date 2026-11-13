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
    private readonly PdfMergeService _pdfMergeService;

    public SyncFunction(
        ILogger<SyncFunction> logger,
        AltinnService altinnService,
        SkarvService skarvService,
        P360Service p360Service,
        PdfMergeService pdfMergeService)
    {
        _logger = logger;
        _altinnService = altinnService;
        _skarvService = skarvService;
        _p360Service = p360Service;
        _pdfMergeService = pdfMergeService;
    }

    // Runs at 00:00, 08:00 and 14:00 Norwegian time every day (winter-time +1)
    [Function("SyncAltinnToP360Timer")]
    public async Task RunTimer([TimerTrigger("0 0 23,7,13 * * *")] TimerInfo timer)
    {
        _logger.LogInformation($"Scheduled sync started at {DateTime.UtcNow} (UTC)");

        var result = await Sync();

        if (result is ObjectResult objectResult)
        {
            _logger.LogInformation($"Scheduled sync finished with status {objectResult.StatusCode}: {JsonSerializer.Serialize(objectResult.Value)}");
        }

        if (timer.ScheduleStatus is not null)
        {
            _logger.LogInformation($"Next scheduled sync: {timer.ScheduleStatus.Next}");
        }
    }

    [Function("SyncAltinnToP360")]
    public Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        => Sync();

    private async Task<IActionResult> Sync()
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

        int processedRecords = 0;
        var failures = new List<object>();

        foreach (var item in altinnItems)
        {
            try
            {
                await ProcessItem(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process Altinn instance {item.InstanceId} (party {item.PartyId}). Skipping and continuing with the rest.");
                failures.Add(new { item.InstanceId, item.PartyId, Error = ex.Message });
                continue;
            }

            processedRecords++;
        }

        if (failures.Count > 0)
        {
            return new ObjectResult(new {
                Message = "Completed with failures",
                RecordsProcessed = processedRecords,
                RecordsFailed = failures.Count,
                Failures = failures,
                Details = "Some instances failed and were not marked complete. They will be retried on the next run."
            }) { StatusCode = StatusCodes.Status207MultiStatus };
        }

        return new OkObjectResult(new {
            Message = "Success",
            RecordsProcessed = processedRecords,
            Details = "Transfer from Altinn to P360 succeeded!"
        });
    }

    private async Task ProcessItem(AltinnResult item)
    {
        {
            // SKARV
            _logger.LogInformation("Fetching each Altinn person via Skarv from APIM");
            
            var ssn = item?.FormData?.personalia?.ssn?.ToString() ?? string.Empty;
            SkarvPerson? skarvResult = await _skarvService.GetPersonWithSSN(ssn);

            string name;
            UnregisteredDocumentContact? unregisteredDocContact = null;

            if (skarvResult == null)
            {
                _logger.LogInformation("No person found in Skarv with SSN. Proceeding with unregistered contact.");

                name = item?.FormData?.personalia?.name ?? "Uregistrert person";

                unregisteredDocContact = new UnregisteredDocumentContact
                {
                    ContactName = name,
                    Role = "recno:5", // "Sender" / "Avsender"
                    ReferenceNumber = !string.IsNullOrEmpty(ssn) ? ssn : null
                };
            }
            else
            {
                name = skarvResult.Fornavn + " " + skarvResult.Etternavn;
            }

            // P360
            _logger.LogInformation("Creating new case to P360 via APIM");
            P360CaseResponse? caseResult = await _p360Service.PostCase(name);

            _logger.LogInformation("Creating new document to P360 via APIM");
            P360DocumentResponse? documentResult = await _p360Service.PostDocument(name, caseResult?.CaseNumber, unregisteredDocContact, skarvResult?.Id360);

            if (item?.Files != null && item.Files.Any())
            {
                _logger.LogInformation("Merging and creating new file to P360 via APIM");
                var mergedBytes = _pdfMergeService.MergePdfDocuments(item.Files);

                if (mergedBytes != null && mergedBytes.Length > 0)
                {
                    var fileName = item.Files.Count == 1 ? item.Files[0].Filename : $"Søknad - {name}.pdf";
                    var content = Convert.ToBase64String(mergedBytes);

                    await _p360Service.PostFile(fileName, documentResult?.DocumentNumber, content);
                }
            }

            // ALTINN
            if (!string.IsNullOrEmpty(item?.InstanceId))
            {
                await _altinnService.CompleteInstance(item.InstanceId);
            }
        }
    }
}